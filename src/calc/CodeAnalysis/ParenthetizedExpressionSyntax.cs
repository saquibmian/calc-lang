using System.Collections.Generic;

namespace CalcLang.CodeAnalysis {
    public sealed class ParenthetizedExpressionSyntax : ExpressionSyntax {
        internal ParenthetizedExpressionSyntax( SyntaxToken openParenthesis, ExpressionSyntax expression, SyntaxToken closeParenthesis ) {
            Expression = expression;
        }

        public ExpressionSyntax Expression { get; }

        public override SyntaxKind Kind => SyntaxKind.ParenthetizedExpression;

        public override IEnumerable<SyntaxNode> ChildNodes() {
            yield return Expression;
        }

        public override string ToString() {
            // TODO(snm): make trivia real
            return $"( {Expression} )";
        }
    }
}
