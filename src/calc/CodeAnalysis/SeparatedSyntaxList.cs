using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CalcLang.CodeAnalysis {
    internal sealed class SeparatedSyntaxList<T> : SyntaxNode where T : SyntaxNode {
        internal SeparatedSyntaxList( IEnumerable<T> nodes, IEnumerable<SyntaxToken> separators ) {
            Nodes = nodes;
            Separators = separators;
        }

        internal IEnumerable<T> Nodes { get; }
        internal IEnumerable<SyntaxToken> Separators { get; }

        internal override SyntaxKind Kind => SyntaxKind.SeparatedSyntaxList;

        internal override IEnumerable<SyntaxNode> ChildNodes() {
            return Nodes;
        }

        public override string ToString() {
            // TODO(snm): make trivia real
            var builder = new StringBuilder();

            var nodes = Nodes.GetEnumerator();
            var separators = Separators.GetEnumerator();
            while ( nodes.MoveNext() ) {
                builder.Append( nodes.Current );
                if ( separators.MoveNext() ) {
                    builder.Append( separators.Current.Text );
                    builder.Append( " " );
                }
            }

            return builder.ToString();
        }
    }
}
