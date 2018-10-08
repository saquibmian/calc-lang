using System.Collections.Generic;

namespace CalcLang.CodeAnalysis {
    internal class MemberAccessExpressionSyntax : ExpressionSyntax {

        public MemberAccessExpressionSyntax( SyntaxToken memberName ) {
            MemberName = memberName;
        }

        internal override SyntaxKind Kind => SyntaxKind.MemberAccessExpression;

        internal SyntaxToken MemberName { get; }

        internal override IEnumerable<SyntaxNode> ChildNodes() {
            yield break;
        }

        public override string ToString() {
            return MemberName.ValueText;
        }
    }
}