using System.Collections.Generic;

namespace CalcLang.CodeAnalysis {
    internal sealed class StatementSyntax : SyntaxNode {

        public StatementSyntax( ExpressionSyntax expression ) {
            Expression = expression;
        }

        public ExpressionSyntax Expression { get; }

        internal override SyntaxKind Kind => SyntaxKind.Statement;

        internal override IEnumerable<SyntaxNode> ChildNodes() {
            yield return Expression;
        }

        public override string ToString() {
            return Expression.ToString();
        }
    }
}
