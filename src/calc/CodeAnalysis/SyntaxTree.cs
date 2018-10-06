using System.Collections.Immutable;

namespace CalcLang.CodeAnalysis {
    internal sealed class SyntaxTree {
        public SyntaxTree( StatementSyntax root, SyntaxToken endOfFileToken, ImmutableArray<Diagnostic> diagnostics ) {
            Root = root;
            Diagnostics = diagnostics;
        }

        public StatementSyntax Root { get; }
        public ImmutableArray<Diagnostic> Diagnostics { get; }
    }
}
