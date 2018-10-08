using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace CalcLang {
    internal sealed class Runtime {

        internal static Runtime Global {
            get {
                var runtime = new Runtime();
                runtime.SetVariable( "PI", (int)Math.PI );
                return runtime;
            }
        }

        private readonly Runtime _parent;
        private readonly Dictionary<string, int> _variablesByName = new Dictionary<string, int>();

        private Runtime( Runtime parent = null ) {
            _parent = parent;
        }

        internal bool HasVariable( string name ) {
            if ( _variablesByName.ContainsKey( name ) ) {
                return true;
            }
            return _parent != null && _parent.HasVariable( name );
        }

        internal void SetVariable( string name, int value ) {
            if ( _parent != null && _parent.HasVariable( name ) ) {
                throw new Exception( $"The variable '{name}' exists in a parent scope already." );
            }
            _variablesByName[name] = value;
        }

        internal bool TryGetVariableValue( string name, out int value ) {
            if ( _variablesByName.TryGetValue( name, out value ) ) {
                return true;
            }
            if ( _parent != null ) {
                return _parent.TryGetVariableValue( name, out value );
            }
            return false;
        }

        internal Runtime CreateScope() {
            return new Runtime( this );
        }
    }
}
