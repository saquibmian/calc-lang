using System;
using System.Collections.Immutable;

namespace CalcLang {
    public sealed class SumMethod : MethodSymbol {
        public override string Name => "sum";

        public override ImmutableArray<ParameterSymbol> Parameters => ImmutableArray.Create<ParameterSymbol>(
            // TODO: varags
        );

        public override Type ReturnType { get; } = typeof( int );

        public override object Execute( Runtime runtime ) {
            // TODO: make this work
            return null;
        }
    }
}
