using System;
using System.Collections.Immutable;

namespace CalcLang {
    internal sealed class SumMethod : RuntimeMethod {
        internal override string Name => "sum";

        internal override ImmutableArray<Parameter> Parameters => ImmutableArray.Create<Parameter>(
            // TODO: varags
        );

        internal override Type ReturnType { get; } = typeof( int );

        internal override object Execute( Runtime runtime ) {
            // TODO: make this work
            return null;
        }
    }
}
