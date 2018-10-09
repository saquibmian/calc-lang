using System.Collections.Generic;

namespace CalcLang.CodeAnalysis {
    public class MemberAccessExpressionSyntax : ExpressionSyntax {

        internal MemberAccessExpressionSyntax( SyntaxToken memberName ) {
            MemberName = memberName;
        }

        public override SyntaxKind Kind => SyntaxKind.MemberAccessExpression;

        public SyntaxToken MemberName { get; }

        public override IEnumerable<SyntaxNode> ChildNodes() {
            yield break;
        }

        public override string ToString() {
            return MemberName.ValueText;
        }
    }
}