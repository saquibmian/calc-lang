using System.Collections.Generic;

namespace CalcLang.CodeAnalysis {
    internal sealed class NumberExpressionSyntax : ExpressionSyntax {
        public NumberExpressionSyntax( SyntaxToken numberToken ) {
            NumberToken = numberToken;
        }

        public SyntaxToken NumberToken { get; }

        internal override SyntaxKind Kind => SyntaxKind.NumberExpression;

        internal override IEnumerable<SyntaxNode> ChildNodes() {
            yield break;
        }

        public override string ToString() {
            return NumberToken.ValueText;
        }
    }
}
