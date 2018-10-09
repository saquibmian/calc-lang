using System;

namespace CalcLang {
    public sealed class Parameter {
        public Parameter( string name, Type type ) {
            if ( string.IsNullOrWhiteSpace( name ) ) {
                throw new ArgumentException( "message", nameof( name ) );
            }

            Name = name;
            Type = type ?? throw new ArgumentNullException( nameof( type ) );
        }

        public string Name { get; }
        public Type Type { get; }
    }
}
