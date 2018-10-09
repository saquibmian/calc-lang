using System;
using System.Collections.Immutable;

namespace CalcLang {
    internal sealed class AddInt32sMethod : RuntimeMethod {
        internal override string Name => "+";

        internal override ImmutableArray<Parameter> Parameters => ImmutableArray.Create<Parameter>(
            new Parameter( "lhs", typeof( int ) ),
            new Parameter( "rhs", typeof( int ) )
        );

        internal override Type ReturnType { get; } = typeof( int );

        internal override object Execute( Runtime runtime ) {
            GetArguments( runtime, out int lhs, out int rhs );
            return lhs + rhs;
        }
    }
}
