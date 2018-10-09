using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CalcLang.CodeAnalysis {
    public sealed class SeparatedSyntaxList<T> : SyntaxNode where T : SyntaxNode {
        internal SeparatedSyntaxList( IEnumerable<T> nodes, IEnumerable<SyntaxToken> separators ) {
            Nodes = nodes;
            Separators = separators;
        }

        public IEnumerable<T> Nodes { get; }
        public IEnumerable<SyntaxToken> Separators { get; }

        public override SyntaxKind Kind => SyntaxKind.SeparatedSyntaxList;

        public override IEnumerable<SyntaxNode> ChildNodes() {
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
                    builder.Append( separators.Current.ValueText );
                    builder.Append( " " );
                }
            }

            return builder.ToString();
        }
    }
}
