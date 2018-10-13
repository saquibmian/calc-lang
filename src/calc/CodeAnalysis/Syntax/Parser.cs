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
                _diagnostics.Add( new Diagnostic( Current.Position, $"Unexpected token <{Current.Kind}>, expected <{kind}>" ) );
                token = new SyntaxToken( kind, Current.Position, null, null );
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
                    return new BooleanLiteralExpressionSyntax( Current, Current.Kind == SyntaxKind.TrueKeyword );

                case SyntaxKind.IdentiferToken:
                    var memberName = Expect( SyntaxKind.IdentiferToken );
                    var memberAccess = new MemberAccessExpressionSyntax( memberName );
                    if ( Current.Kind != SyntaxKind.OpenParenthesisToken ) {
                        return memberAccess;
                    }
                    var arguments = ParseArgumentList();
                    return new InvocationExpressionSyntax( memberAccess, arguments );

                case SyntaxKind.OpenParenthesisToken:
                    var open = Expect( SyntaxKind.OpenParenthesisToken );
                    var expression = ParseExpression();
                    var close = Expect( SyntaxKind.CloseParenthesisToken );
                    return new ParenthetizedExpressionSyntax( open, expression, close );

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
