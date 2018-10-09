using System;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;

namespace CalcLang {
    internal abstract class RuntimeMethod {
        internal abstract string Name { get; }
        internal abstract ImmutableArray<Parameter> Parameters { get; }
        internal abstract Type ReturnType { get; }
        internal abstract object Execute( Runtime runtime );

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        private object GetArgument( Runtime runtime, Parameter p ) {
            if ( runtime.TryGetVariableValue( p.Name, out var arg ) ) {
                return arg;
            }
            throw new Exception( $"Missing argument {p.Name}" );
        }

        protected void GetArguments<T1>( Runtime runtime, out T1 arg1 ) {
            arg1 = (T1)GetArgument( runtime, Parameters[0] );
        }
        protected void GetArguments<T1,T2>( Runtime runtime, out T1 arg1, out T2 arg2 ) {
            arg1 = (T1)GetArgument( runtime, Parameters[0] );
            arg2 = (T2)GetArgument( runtime, Parameters[1] );
        }
        protected void GetArguments<T1,T2,T3>( Runtime runtime, out T1 arg1, out T2 arg2, out T3 arg3 ) {
            arg1 = (T1)GetArgument( runtime, Parameters[0] );
            arg2 = (T2)GetArgument( runtime, Parameters[1] );
            arg3 = (T3)GetArgument( runtime, Parameters[2] );
        }
    }
}
