using System;

namespace CalcLang.CodeAnalysis.Syntax {
    public sealed class Diagnostic {
        internal Diagnostic(int position, string message) {
            if ( string.IsNullOrWhiteSpace( message ) ) {
                throw new ArgumentException( "message", nameof( message ) );
            }

            Position = position;
            Message = message;
        }

        public int Position { get; }
        public string Message { get; }
    }
}
