using System.Collections.Generic;

namespace CalcLang.CodeAnalysis {
    internal abstract class SyntaxNode {
        internal abstract SyntaxKind Kind { get; }
        internal abstract IEnumerable<SyntaxNode> ChildNodes();
    }
}
