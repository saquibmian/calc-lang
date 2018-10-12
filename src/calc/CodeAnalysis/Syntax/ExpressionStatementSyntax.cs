using System.Collections.Generic;

namespace CalcLang.CodeAnalysis.Syntax {
    public sealed class ExpressionStatementSyntax : StatementSyntax {

        internal ExpressionStatementSyntax( ExpressionSyntax expression ) {
            Expression = expression;
        }

        public ExpressionSyntax Expression { get; }

        public override SyntaxKind Kind => SyntaxKind.ExpressionStatement;

        public override IEnumerable<SyntaxNode> ChildNodes() {
            yield return Expression;
        }

        public override string ToString() {
            return Expression.ToString();
        }
    }
}
