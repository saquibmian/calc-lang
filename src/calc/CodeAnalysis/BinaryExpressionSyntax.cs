using System.Collections.Generic;

namespace CalcLang.CodeAnalysis {
    internal sealed class BinaryExpressionSyntax : ExpressionSyntax {
        public BinaryExpressionSyntax( ExpressionSyntax left, SyntaxToken operatorToken, ExpressionSyntax right ) {
            Left = left;
            OperatorToken = operatorToken;
            Right = right;
        }

        public ExpressionSyntax Left { get; }
        public SyntaxToken OperatorToken { get; }
        public ExpressionSyntax Right { get; }

        internal override SyntaxKind Kind => SyntaxKind.BinaryExpression;

        internal override IEnumerable<SyntaxNode> ChildNodes() {
            yield return Left;
            yield return Right;
        }

        public override string ToString() {
            // TODO(snm): make trivia real
            return $"{Left} {OperatorToken.ValueText} {Right}";
        }
    }
}
