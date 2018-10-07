using System.Collections.Generic;

namespace CalcLang.CodeAnalysis {
    internal sealed class InvocationExpressionSyntax : ExpressionSyntax {
        internal InvocationExpressionSyntax( SyntaxToken identifer, ArgumentListSyntax argumentList ) {
            Identifer = identifer;
            ArgumentList = argumentList;
        }

        internal SyntaxToken Identifer { get; }
        internal ArgumentListSyntax ArgumentList { get; }

        internal override SyntaxKind Kind => SyntaxKind.InvocationExpression;

        internal override IEnumerable<SyntaxNode> ChildNodes() {
            yield return ArgumentList;
        }

        public override string ToString() {
            // TODO(snm): make trivia real
            return $"{Identifer.Text}{ArgumentList}";
        }
    }
}
