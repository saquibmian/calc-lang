using System.Collections.Generic;

namespace CalcLang.CodeAnalysis {
    internal sealed class ArgumentSyntax : SyntaxNode {
        // TODO(snm) add overload with name colon
        public ArgumentSyntax( ExpressionSyntax expression ) {
            Expression = expression;
        }

        public ExpressionSyntax Expression { get; }

        internal override SyntaxKind Kind => SyntaxKind.Argument;

        internal override IEnumerable<SyntaxNode> ChildNodes() {
            yield return Expression;
        }

        public override string ToString() {
            return Expression.ToString();
        }
    }
}
