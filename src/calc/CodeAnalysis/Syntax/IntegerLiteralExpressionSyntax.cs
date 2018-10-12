using System.Collections.Generic;

namespace CalcLang.CodeAnalysis.Syntax {
    public sealed class IntegerLiteralExpressionSyntax : ConstantExpressionSyntax {
        internal IntegerLiteralExpressionSyntax( SyntaxToken numberToken ) {
            NumberToken = numberToken;
        }

        public SyntaxToken NumberToken { get; }

        public override SyntaxKind Kind => SyntaxKind.IntegerLiteralExpression;

        public override object Value => NumberToken.Value;

        public override IEnumerable<SyntaxNode> ChildNodes() {
            yield break;
        }

        public override string ToString() {
            return NumberToken.ValueText;
        }
    }
}
