using System.IO;
using System.Linq;

namespace CalcLang.CodeAnalysis.Syntax {
    internal sealed class SyntaxTreePrinter {

        private readonly StringWriter writer = new StringWriter();

        private SyntaxTreePrinter() { }

        internal static string PrintTree( SyntaxTree tree ) {
            var printer = new SyntaxTreePrinter();
            return printer.Print( tree );
        }

        internal string Print( SyntaxTree tree ) {
            Print( tree.Root, string.Empty, true );
            return writer.ToString();
        }

        internal void Print( SyntaxNode node, string indent, bool isLast ) {
            var marker = isLast ? "└──" : "├──";
            writer.WriteLine( $"{indent}{marker}{node.Kind}: {node}" );

            indent = indent + ( isLast ? "    " : "│   " );
            var children = node.ChildNodes().ToArray();
            for ( int i = 0; i < children.Length; ++i ) {
                var child = children[i];
                Print( child, indent, i == children.Length - 1 );
            }
        }
    }
}
