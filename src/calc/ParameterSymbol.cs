using System;
using CalcLang.CodeAnalysis;
using CalcLang.CodeAnalysis.Syntax;

namespace CalcLang {
    public sealed class ParameterSymbol {
        public ParameterSymbol( string name, Type type, ConstantExpressionSyntax defaultValue = null ) {
            if ( string.IsNullOrWhiteSpace( name ) ) {
                throw new ArgumentException( "message", nameof( name ) );
            }

            Name = name;
            Type = type ?? throw new ArgumentNullException( nameof( type ) );
            DefaultValue = defaultValue;
        }

        public string Name { get; }
        public Type Type { get; }
        public ConstantExpressionSyntax DefaultValue { get; }
    }
}
