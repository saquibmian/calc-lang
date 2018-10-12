using System.Collections.Generic;

namespace CalcLang.CodeAnalysis.Syntax {
    public abstract class SyntaxNode {
        public abstract SyntaxKind Kind { get; }
        public abstract IEnumerable<SyntaxNode> ChildNodes();
    }
}
