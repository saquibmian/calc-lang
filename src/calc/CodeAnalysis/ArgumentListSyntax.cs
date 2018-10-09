using System.Collections.Generic;

namespace CalcLang.CodeAnalysis {
    public sealed class ArgumentListSyntax : SyntaxNode {
        internal ArgumentListSyntax( SyntaxToken openParenthesisToken, SeparatedSyntaxList<ArgumentSyntax> arguments, SyntaxToken closeParenthesisToken ) {
            Arguments = arguments;
        }

        public SeparatedSyntaxList<ArgumentSyntax> Arguments { get; }

        public override SyntaxKind Kind => SyntaxKind.ArgumentList;

        public override IEnumerable<SyntaxNode> ChildNodes() {
            yield return Arguments;
        }
        public override string ToString() {
            // TODO(snm): make trivia real
            return $"( {Arguments} )";
        }
    }
}
