using System.Collections.Generic;

namespace CalcLang.CodeAnalysis {
    public sealed class InvocationExpressionSyntax : ExpressionSyntax {
        internal InvocationExpressionSyntax( MemberAccessExpressionSyntax member, ArgumentListSyntax argumentList ) {
            Member = member;
            ArgumentList = argumentList;
        }

        public MemberAccessExpressionSyntax Member { get; }
        public ArgumentListSyntax ArgumentList { get; }

        public override SyntaxKind Kind => SyntaxKind.InvocationExpression;

        public override IEnumerable<SyntaxNode> ChildNodes() {
            yield return ArgumentList;
        }

        public override string ToString() {
            // TODO(snm): make trivia real
            return $"{Member.MemberName.ValueText}{ArgumentList}";
        }
    }
}
