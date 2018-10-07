using System.Collections.Generic;

namespace CalcLang.CodeAnalysis {
    internal sealed class ArgumentListSyntax : SyntaxNode {
        internal ArgumentListSyntax( SyntaxToken openParenthesisToken, SeparatedSyntaxList<ArgumentSyntax> arguments, SyntaxToken closeParenthesisToken ) {
            Arguments = arguments;
        }

        internal SeparatedSyntaxList<ArgumentSyntax> Arguments { get; }

        internal override SyntaxKind Kind => SyntaxKind.ArgumentList;

        internal override IEnumerable<SyntaxNode> ChildNodes() {
            yield return Arguments;
        }
        public override string ToString() {
            // TODO(snm): make trivia real
            return $"( {Arguments} )";
        }
    }
}
