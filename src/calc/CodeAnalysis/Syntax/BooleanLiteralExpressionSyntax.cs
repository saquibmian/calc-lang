using System.Collections.Generic;
using System.Collections.Immutable;

namespace CalcLang.CodeAnalysis.Syntax {
    internal class BooleanLiteralExpressionSyntax : ConstantExpressionSyntax {
        public BooleanLiteralExpressionSyntax( SyntaxToken token, bool value ) {
            Value = value;
        }

        public override object Value { get; }

        public override SyntaxKind Kind => SyntaxKind.BooleanLiteralExpression;

        public override IEnumerable<SyntaxNode> ChildNodes() {
            yield break;
        }

        public override string ToString() {
            return Value.ToString();
        }
    }
}