using System.Collections.Generic;

namespace CalcLang.CodeAnalysis {
    internal sealed class LocalDeclarationStatementSyntax : StatementSyntax {
        public LocalDeclarationStatementSyntax(SyntaxToken letToken, SyntaxToken nameToken, SyntaxToken equalsToken, ExpressionSyntax expression) {
            NameToken = nameToken;
            Expression = expression;
        }

        public SyntaxToken NameToken { get; }
        public ExpressionSyntax Expression { get; }

        internal override SyntaxKind Kind => SyntaxKind.LocalDeclarationStatement;

        internal override IEnumerable<SyntaxNode> ChildNodes() {
            yield return Expression;
        }

        public override string ToString() {
            return $"let {NameToken.ValueText} = {Expression}";
        }
    }
}
