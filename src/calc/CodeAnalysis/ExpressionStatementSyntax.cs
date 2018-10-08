using System.Collections.Generic;

namespace CalcLang.CodeAnalysis {
    internal sealed class ExpressionStatementSyntax : StatementSyntax {

        public ExpressionStatementSyntax( ExpressionSyntax expression ) {
            Expression = expression;
        }

        public ExpressionSyntax Expression { get; }

        internal override SyntaxKind Kind => SyntaxKind.ExpressionStatement;

        internal override IEnumerable<SyntaxNode> ChildNodes() {
            yield return Expression;
        }

        public override string ToString() {
            return Expression.ToString();
        }
    }
}
