using System.Collections.Generic;

namespace CalcLang.CodeAnalysis {
    public abstract class SyntaxNode {
        public abstract SyntaxKind Kind { get; }
        public abstract IEnumerable<SyntaxNode> ChildNodes();
    }
}
