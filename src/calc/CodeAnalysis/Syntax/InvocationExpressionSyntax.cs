using System.Collections.Generic;

namespace CalcLang.CodeAnalysis.Syntax {
    public sealed class InvocationExpressionSyntax : ExpressionSyntax {
        internal InvocationExpressionSyntax( ExpressionSyntax expression, ArgumentListSyntax argumentList ) {
            Expression = expression;
            ArgumentList = argumentList;
        }

        public ExpressionSyntax Expression { get; }
        public ArgumentListSyntax ArgumentList { get; }

        public override SyntaxKind Kind => SyntaxKind.InvocationExpression;

        public override IEnumerable<SyntaxNode> ChildNodes() {
            yield return Expression;
            yield return ArgumentList;
        }

        public override string ToString() {
            // TODO(snm): make trivia real
            return $"{Expression}{ArgumentList}";
        }
    }
}
