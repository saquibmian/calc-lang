using System;
using System.Collections.Immutable;

namespace CalcLang {
    public sealed class AddInt32sMethod : RuntimeMethod {
        public override string Name => "+";

        public override ImmutableArray<Parameter> Parameters => ImmutableArray.Create<Parameter>(
            new Parameter( "lhs", typeof( int ) ),
            new Parameter( "rhs", typeof( int ) )
        );

        public override Type ReturnType { get; } = typeof( int );

        public override object Execute( Runtime runtime ) {
            GetArguments( runtime, out int lhs, out int rhs );
            return lhs + rhs;
        }
    }
}
