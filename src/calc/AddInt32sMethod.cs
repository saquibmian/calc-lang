using System;
using System.Collections.Immutable;

namespace CalcLang {
    public sealed class AddInt32sMethod : MethodSymbol {
        public override string Name => "+";

        public override ImmutableArray<ParameterSymbol> Parameters => ImmutableArray.Create<ParameterSymbol>(
            new ParameterSymbol( "lhs", typeof( int ) ),
            new ParameterSymbol( "rhs", typeof( int ) )
        );

        public override Type ReturnType { get; } = typeof( int );

        public override object Execute( Runtime runtime ) {
            GetArguments( runtime, out int lhs, out int rhs );
            return lhs + rhs;
        }
    }
}
