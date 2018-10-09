using System.Collections.Generic;

namespace CalcLang.CodeAnalysis {
    public sealed class FloatLiteralExpressionSyntax : ExpressionSyntax {
        internal FloatLiteralExpressionSyntax( SyntaxToken numberToken ) {
            NumberToken = numberToken;
        }

        public SyntaxToken NumberToken { get; }

        public override SyntaxKind Kind => SyntaxKind.FloatLiteralExpression;

        public override IEnumerable<SyntaxNode> ChildNodes() {
            yield break;
        }

        public override string ToString() {
            return NumberToken.ValueText;
        }
    }
}
