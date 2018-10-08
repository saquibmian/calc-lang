using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace CalcLang {
    internal sealed class Runtime {
        private readonly ImmutableDictionary<string, int> _immutableVariables = new Dictionary<string, int> {
            ["PI"] = (int)Math.PI
        }.ToImmutableDictionary();

        private readonly Dictionary<string, int> _variablesByName = new Dictionary<string, int>();

        internal void SetVariable( string name, int value ) {
            if ( _immutableVariables.ContainsKey( name ) ) {
                throw new Exception( "Cannot stomp global variable." );
            }
            _variablesByName[name] = value;
        }

        internal bool TryGetVariableValue( string name, out int value ) {
            if ( _variablesByName.TryGetValue( name, out value ) ) {
                return true;
            }
            return _immutableVariables.TryGetValue( name, out value );
        }
    }
}
