using System;

namespace CalcLang.CodeAnalysis {
    internal sealed class Diagnostic {
        public Diagnostic(int position, string message) {
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
