using System.Collections.Generic;

namespace CalcLang.CodeAnalysis.Syntax {
    public class MemberAccessExpressionSyntax : ExpressionSyntax {

        internal MemberAccessExpressionSyntax( SyntaxToken memberName ) {
            MemberName = memberName;
        }
        internal MemberAccessExpressionSyntax( ExpressionSyntax expression, SyntaxToken dotToken, SyntaxToken memberName ) {
            Expression = expression;
            MemberName = memberName;
        }

        public override SyntaxKind Kind => SyntaxKind.MemberAccessExpression;

        public ExpressionSyntax Expression { get; }
        public SyntaxToken MemberName { get; }

        public override IEnumerable<SyntaxNode> ChildNodes() {
            if ( Expression != null ) {
                yield return Expression;
            }
        }

        public override string ToString() {
            if ( Expression != null ) {
                return $"{Expression}.{MemberName.ValueText}";
            }
            return MemberName.ValueText;
        }
    }
}