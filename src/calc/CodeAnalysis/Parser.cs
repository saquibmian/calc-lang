using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace CalcLang.CodeAnalysis {

    public sealed class Parser {

        private readonly List<Diagnostic> _diagnostics = new List<Diagnostic>();
        private readonly ImmutableArray<SyntaxToken> _tokens;

        private int _position;

        private Parser( string input ) {
            var lexer = new Lexer( input );
            _tokens = lexer.ReadAllTokens();
            _diagnostics.AddRange( lexer.Diagnostics );
        }

        public static SyntaxTree Parse( string input ) {
            var parser = new Parser( input );
            return parser.ParseSyntaxTree();
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
            var statement = ParseStatement();
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

        private StatementSyntax ParseStatement() {
            if ( Current.Kind == SyntaxKind.IdentiferToken ) {
                switch ( Current.Value ) {
                    case "let":
                        var letToken = Expect( SyntaxKind.IdentiferToken );
                        var varName = Expect( SyntaxKind.IdentiferToken );
                        var equalsToken = Expect( SyntaxKind.EqualsToken );
                        var varValue = ParseTerm();
                        return new LocalDeclarationStatementSyntax( letToken, varName, equalsToken, varValue );
                }
            }

            var expression = ParseTerm();
            return new ExpressionStatementSyntax( expression );
        }

        private ExpressionSyntax ParseTerm() {
            var left = ParseFactor();

            while ( Current.Kind == SyntaxKind.PlusToken || Current.Kind == SyntaxKind.MinusToken ) {
                var operatorToken = Current;
                MoveNext();
                var right = ParseFactor();
                left = new BinaryExpressionSyntax( left, operatorToken, right );
            }

            return left;
        }

        private ExpressionSyntax ParseFactor() {
            var left = ParsePrimaryExpression();

            while ( Current.Kind == SyntaxKind.StarToken || Current.Kind == SyntaxKind.ForwardSlashToken ) {
                var operatorToken = Current;
                MoveNext();
                var right = ParsePrimaryExpression();
                left = new BinaryExpressionSyntax( left, operatorToken, right );
            }

            return left;
        }

        private ExpressionSyntax ParsePrimaryExpression() {
            if ( Current.Kind == SyntaxKind.IdentiferToken ) {
                var memberName = Expect( SyntaxKind.IdentiferToken );
                if ( Current.Kind == SyntaxKind.OpenParenthesisToken ) {
                    var arguments = ParseArgumentList();
                    return new InvocationExpressionSyntax( memberName, arguments );
                }
                return new MemberAccessExpressionSyntax( memberName );
            }

            if ( Current.Kind == SyntaxKind.OpenParenthesisToken ) {
                var open = Expect( SyntaxKind.OpenParenthesisToken );
                var expression = ParseTerm();
                var close = Expect( SyntaxKind.CloseParenthesisToken );
                return new ParenthetizedExpressionSyntax( open, expression, close );
            }

            var token = Expect( SyntaxKind.IntegerToken );
            return new IntegerLiteralExpressionSyntax( token );
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
            var expression = ParseTerm();
            return new ArgumentSyntax( expression );
        }

    }
}
