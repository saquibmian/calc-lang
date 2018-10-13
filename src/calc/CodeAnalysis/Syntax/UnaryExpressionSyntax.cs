using System.Collections.Generic;

namespace CalcLang.CodeAnalysis.Syntax {
    public sealed class UnaryExpressionSyntax : ExpressionSyntax {
        public UnaryExpressionSyntax( SyntaxToken operand, ExpressionSyntax expression ) {
            OperatorToken = operand;
            Expression = expression;
        }

        public override SyntaxKind Kind => SyntaxKind.UnaryExpression;

        public SyntaxToken OperatorToken { get; }
        public ExpressionSyntax Expression { get; }

        public override IEnumerable<SyntaxNode> ChildNodes() {
            yield return Expression;
        }
    }
}
