using System.Collections.Generic;

namespace CalcLang.CodeAnalysis.Syntax {
    public sealed class FloatLiteralExpressionSyntax : ConstantExpressionSyntax {
        internal FloatLiteralExpressionSyntax( SyntaxToken numberToken ) {
            NumberToken = numberToken;
        }

        public SyntaxToken NumberToken { get; }

        public override SyntaxKind Kind => SyntaxKind.FloatLiteralExpression;

        public override object Value => NumberToken.Value;

        public override IEnumerable<SyntaxNode> ChildNodes() {
            yield break;
        }

        public override string ToString() {
            return NumberToken.ValueText;
        }
    }
}
