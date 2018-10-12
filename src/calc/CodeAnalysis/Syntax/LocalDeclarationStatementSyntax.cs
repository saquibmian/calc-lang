using System.Collections.Generic;

namespace CalcLang.CodeAnalysis.Syntax {
    public sealed class LocalDeclarationStatementSyntax : StatementSyntax {
        internal LocalDeclarationStatementSyntax(SyntaxToken letToken, SyntaxToken nameToken, SyntaxToken equalsToken, ExpressionSyntax expression) {
            NameToken = nameToken;
            Expression = expression;
        }

        public SyntaxToken NameToken { get; }
        public ExpressionSyntax Expression { get; }

        public override SyntaxKind Kind => SyntaxKind.LocalDeclarationStatement;

        public override IEnumerable<SyntaxNode> ChildNodes() {
            yield return Expression;
        }

        public override string ToString() {
            return $"let {NameToken.ValueText} = {Expression}";
        }
    }
}
