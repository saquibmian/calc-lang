using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace CalcLang.CodeAnalysis.Syntax {

    internal sealed class Parser {

        private readonly List<Diagnostic> _diagnostics = new List<Diagnostic>();
        private readonly ImmutableArray<SyntaxToken> _tokens;

        private int _position;

        internal Parser( string input ) {
            var lexer = new Lexer( input );
            _tokens = lexer.ReadAllTokens();
            _diagnostics.AddRange( lexer.Diagnostics );
        }

        private SyntaxToken Current {
            get {
                var index = _position;
                if ( index >= _tokens.Length ) {
                    return _tokens.Last();
                }

                return _tokens[index];
            }
        }

        private void MoveNext() => ++_position;

        internal SyntaxTree ParseSyntaxTree() {
            var statement = ParseExpression();
            var eofToken = Expect( SyntaxKind.EndOfFileToken );

            return new SyntaxTree( statement, eofToken, _diagnostics.ToImmutableArray() );
        }

        private SyntaxToken Expect( SyntaxKind kind ) {
            var token = Current;
            if ( token.Kind != kind ) {
                _diagnostics.Add( new Diagnostic( Current.Location, $"Unexpected token <{Current.Kind}>, expected <{kind}>" ) );
                token = new SyntaxToken( kind, Current.Location, null, null );
            }

            MoveNext();
            return token;
        }

        private ExpressionSyntax ParseExpression( int parentPrecendence = 0 ) {
            ExpressionSyntax left;

            var unaryPrecendence = Current.GetUnaryOperatorPrecendence();
            if ( unaryPrecendence != 0 && unaryPrecendence >= parentPrecendence ) {
                var operand = Current;
                MoveNext();
                var expr = ParseExpression( unaryPrecendence );
                left = new UnaryExpressionSyntax( operand, expr );
            } else {
                left = ParsePrimaryExpression();
            }

            while ( true ) {
                var currentPrecedence = Current.GetBinaryOperatorPrecendence();
                if ( currentPrecedence <= parentPrecendence ) {
                    // We don't bind as strongly as the parent, so let the parent parse it
                    break;
                }

                var operand = Current;
                MoveNext();
                var right = ParseExpression( currentPrecedence );
                left = new BinaryExpressionSyntax( left, operand, right );
            }

            return left;
        }

        private ExpressionSyntax ParsePrimaryExpression() {
            switch ( Current.Kind ) {
                case SyntaxKind.TrueKeyword:
                case SyntaxKind.FalseKeyword:
                    var boolToken = Current;
                    MoveNext();
                    return new BooleanLiteralExpressionSyntax( boolToken, boolToken.Kind == SyntaxKind.TrueKeyword );

                case SyntaxKind.IdentiferToken:
                    var memberName = Expect( SyntaxKind.IdentiferToken );
                    ExpressionSyntax expr = new MemberAccessExpressionSyntax( memberName );
                    while ( Current.Kind == SyntaxKind.DotToken || Current.Kind == SyntaxKind.OpenParenthesisToken ) {
                        if ( Current.Kind == SyntaxKind.DotToken ) {
                            var dotToken = Expect( SyntaxKind.DotToken );
                            memberName = Expect( SyntaxKind.IdentiferToken );
                            expr = new MemberAccessExpressionSyntax( expr, dotToken, memberName );
                        }
                        if ( Current.Kind == SyntaxKind.OpenParenthesisToken ) {
                            var arguments = ParseArgumentList();
                            expr = new InvocationExpressionSyntax( expr, arguments );
                        }
                    }
                    return expr;

                case SyntaxKind.OpenParenthesisToken:
                    var open = Expect( SyntaxKind.OpenParenthesisToken );
                    var expression = ParseExpression();
                    var close = Expect( SyntaxKind.CloseParenthesisToken );
                    expression = new ParenthetizedExpressionSyntax( open, expression, close );
                    while ( Current.Kind == SyntaxKind.DotToken || Current.Kind == SyntaxKind.OpenParenthesisToken ) {
                        if ( Current.Kind == SyntaxKind.DotToken ) {
                            var dotToken = Expect( SyntaxKind.DotToken );
                            memberName = Expect( SyntaxKind.IdentiferToken );
                            expression = new MemberAccessExpressionSyntax( expression, dotToken, memberName );
                        }
                        if ( Current.Kind == SyntaxKind.OpenParenthesisToken ) {
                            var arguments = ParseArgumentList();
                            expression = new InvocationExpressionSyntax( expression, arguments );
                        }
                    }
                    return expression;

                case SyntaxKind.IntegerToken:
                    var intToken = Expect( SyntaxKind.IntegerToken );
                    return new IntegerLiteralExpressionSyntax( intToken );

                case SyntaxKind.FloatToken:
                    var floatToken = Expect( SyntaxKind.FloatToken );
                    return new FloatLiteralExpressionSyntax( floatToken );
            }

            throw new Exception( $"Unhandled primary expression kind: {Current.Kind}" );
        }

        private ArgumentListSyntax ParseArgumentList() {
            var open = Expect( SyntaxKind.OpenParenthesisToken );

            var args = new List<ArgumentSyntax>();
            var separators = new List<SyntaxToken>();
            while ( Current.Kind != SyntaxKind.CloseParenthesisToken ) {
                var arg = ParseArgument();
                args.Add( arg );

                if ( Current.Kind == SyntaxKind.CloseParenthesisToken ) {
                    break;
                }

                var separator = Expect( SyntaxKind.CommaToken );
                separators.Add( separator );
            }
            var arguments = new SeparatedSyntaxList<ArgumentSyntax>( args, separators );

            var close = Expect( SyntaxKind.CloseParenthesisToken );

            return new ArgumentListSyntax( open, arguments, close );
        }

        private ArgumentSyntax ParseArgument() {
            var expression = ParseExpression();
            return new ArgumentSyntax( expression );
        }

    }
}
