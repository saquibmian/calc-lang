using System.Collections.Generic;

namespace CalcLang.CodeAnalysis {
    internal sealed class ParenthetizedExpressionSyntax : ExpressionSyntax {
        internal ParenthetizedExpressionSyntax( SyntaxToken openParenthesis, ExpressionSyntax expression, SyntaxToken closeParenthesis ) {
            Expression = expression;
        }

        internal ExpressionSyntax Expression { get; }

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
