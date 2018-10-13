using System.Collections.Immutable;

namespace CalcLang.CodeAnalysis.Syntax {
    public sealed class SyntaxTree {
        internal SyntaxTree( SyntaxNode root, SyntaxToken endOfFileToken, ImmutableArray<Diagnostic> diagnostics ) {
            Root = root;
            Diagnostics = diagnostics;
        }

        public SyntaxNode Root { get; }
        public ImmutableArray<Diagnostic> Diagnostics { get; }

        public static SyntaxTree Parse( string input ) {
            var parser = new Parser( input );
            return parser.ParseSyntaxTree();
        }
    }
}
