using System;
using System.Collections.Immutable;

namespace CalcLang {
    public sealed class SumMethod : RuntimeMethod {
        public override string Name => "sum";

        public override ImmutableArray<Parameter> Parameters => ImmutableArray.Create<Parameter>(
            // TODO: varags
        );

        public override Type ReturnType { get; } = typeof( int );

        public override object Execute( Runtime runtime ) {
            // TODO: make this work
            return null;
        }
    }
}
