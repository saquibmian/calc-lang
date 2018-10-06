using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace CalcLang.CodeAnalysis {

    internal sealed class SyntaxParser {

        private readonly List<Diagnostic> _diagnostics = new List<Diagnostic>();
        private readonly ImmutableArray<SyntaxToken> _tokens;

        private int _position;

        private SyntaxParser( string input ) {
            var lexer = new Lexer( input );
            _tokens = lexer.ReadAllTokens();
            _diagnostics.AddRange( lexer.Diagnostics );
        }

        internal static SyntaxTree Parse( string input ) {
            var parser = new SyntaxParser( input );
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
            var expression = ParseTerm();
            return new StatementSyntax( expression );
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
            if ( Current.Kind == SyntaxKind.OpenParenthesisToken ) {
                var open = Expect( SyntaxKind.OpenParenthesisToken );
                var expression = ParseTerm();
                var close = Expect( SyntaxKind.CloseParenthesisToken );
                return new ParenthetizedExpressionSyntax( open, expression, close );
            }

            var token = Expect( SyntaxKind.IntegerToken );
            return new NumberExpressionSyntax( token );
        }

    }
}
