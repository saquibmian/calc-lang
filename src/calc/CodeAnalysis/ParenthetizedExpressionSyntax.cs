using System.Collections.Generic;

namespace CalcLang.CodeAnalysis {
    internal sealed class ParenthetizedExpressionSyntax : ExpressionSyntax {
        public ParenthetizedExpressionSyntax( SyntaxToken openParenthesis, ExpressionSyntax expression, SyntaxToken closeParenthesis ) {
            Expression = expression;
        }

        public ExpressionSyntax Expression { get; }

        internal override SyntaxKind Kind => SyntaxKind.ParenthetizedExpression;

        internal override IEnumerable<SyntaxNode> ChildNodes() {
            yield return Expression;
        }

        public override string ToString() {
            // TODO(snm): make trivia real
            return $"( {Expression} )";
        }
    }
}
