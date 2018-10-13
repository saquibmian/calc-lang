using System;

namespace CalcLang.CodeAnalysis.Binding {
    public abstract class BoundExpression : BoundNode {
        public abstract Type ReturnType { get; }
    }

}