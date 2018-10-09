using System;
using System.Collections.Generic;
using System.Linq;

namespace CalcLang {
    internal sealed class Runtime {

        private static Lazy<Runtime> _globalRuntime = new Lazy<Runtime>( () => {
            var runtime = new Runtime();
            runtime.SetVariable( "PI", Math.PI );
            runtime.AddMethod( new AddInt32sMethod() );
            runtime.AddMethod( new SumMethod() );
            return runtime;
        } );

        internal static Runtime Global => _globalRuntime.Value;

        private readonly Runtime _parent;
        private readonly Dictionary<string, object> _variablesByName = new Dictionary<string, object>();
        private readonly List<RuntimeMethod> _methods = new List<RuntimeMethod>();

        private Runtime( Runtime parent = null ) {
            _parent = parent;
        }

        internal bool HasVariable( string name ) {
            if ( _variablesByName.ContainsKey( name ) ) {
                return true;
            }
            return _parent != null && _parent.HasVariable( name );
        }

        internal void SetVariable( string name, object value ) {
            if ( _parent != null && _parent.HasVariable( name ) ) {
                throw new Exception( $"The variable '{name}' exists in a parent scope already." );
            }
            _variablesByName[name] = value;
        }

        internal bool TryGetVariableValue( string name, out object value ) {
            if ( _variablesByName.TryGetValue( name, out value ) ) {
                return true;
            }
            if ( _parent != null ) {
                return _parent.TryGetVariableValue( name, out value );
            }
            return false;
        }

        internal void AddMethod( RuntimeMethod method ) {
            var existing = GetMethod( method.Name, method.Parameters.Select( p => p.Type ).ToArray() );
            if ( existing != null ) {
                throw new Exception( "Function already defined." );
            }

            _methods.Add( method );
        }

        internal RuntimeMethod GetMethod( string name, Type[] parameterTypes ) {
            var candidate = _methods.FirstOrDefault( m => {
                return m.Name == name && parameterTypes.SequenceEqual( m.Parameters.Select( p => p.Type ) );
            } );

            return candidate ?? _parent?.GetMethod( name, parameterTypes );
        }

        internal Runtime CreateScope() {
            return new Runtime( this );
        }
    }
}
