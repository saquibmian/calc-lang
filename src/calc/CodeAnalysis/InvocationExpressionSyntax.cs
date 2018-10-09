using System.Collections.Generic;

namespace CalcLang.CodeAnalysis {
    public sealed class InvocationExpressionSyntax : ExpressionSyntax {
        internal InvocationExpressionSyntax( SyntaxToken identifer, ArgumentListSyntax argumentList ) {
            Identifer = identifer;
            ArgumentList = argumentList;
        }

        public SyntaxToken Identifer { get; }
        public ArgumentListSyntax ArgumentList { get; }

        public override SyntaxKind Kind => SyntaxKind.InvocationExpression;

        public override IEnumerable<SyntaxNode> ChildNodes() {
            yield return ArgumentList;
        }

        public override string ToString() {
            // TODO(snm): make trivia real
            return $"{Identifer.ValueText}{ArgumentList}";
        }
    }
}
