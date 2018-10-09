using System.Collections.Generic;

namespace CalcLang.CodeAnalysis {
    public sealed class NumberExpressionSyntax : ExpressionSyntax {
        internal NumberExpressionSyntax( SyntaxToken numberToken ) {
            NumberToken = numberToken;
        }

        public SyntaxToken NumberToken { get; }

        public override SyntaxKind Kind => SyntaxKind.NumberExpression;

        public override IEnumerable<SyntaxNode> ChildNodes() {
            yield break;
        }

        public override string ToString() {
            return NumberToken.ValueText;
        }
    }
}
