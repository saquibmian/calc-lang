using System.Collections.Generic;

namespace CalcLang.CodeAnalysis {
    public sealed class ArgumentSyntax : SyntaxNode {
        // TODO(snm) add overload with name colon
        internal ArgumentSyntax( ExpressionSyntax expression ) {
            Expression = expression;
        }

        public ExpressionSyntax Expression { get; }

        public override SyntaxKind Kind => SyntaxKind.Argument;

        public override IEnumerable<SyntaxNode> ChildNodes() {
            yield return Expression;
        }

        public override string ToString() {
            return Expression.ToString();
        }
    }
}
