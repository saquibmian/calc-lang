using System.Collections.Generic;

namespace CalcLang.CodeAnalysis {
    public sealed class IntegerLiteralExpressionSyntax : ExpressionSyntax {
        internal IntegerLiteralExpressionSyntax( SyntaxToken numberToken ) {
            NumberToken = numberToken;
        }

        public SyntaxToken NumberToken { get; }

        public override SyntaxKind Kind => SyntaxKind.IntegerLiteralExpression;

        public override IEnumerable<SyntaxNode> ChildNodes() {
            yield break;
        }

        public override string ToString() {
            return NumberToken.ValueText;
        }
    }
}
