using System.Collections.Immutable;

namespace CalcLang.CodeAnalysis {
    public sealed class SyntaxTree {
        internal SyntaxTree( StatementSyntax root, SyntaxToken endOfFileToken, ImmutableArray<Diagnostic> diagnostics ) {
            Root = root;
            Diagnostics = diagnostics;
        }

        public StatementSyntax Root { get; }
        public ImmutableArray<Diagnostic> Diagnostics { get; }
    }
}
