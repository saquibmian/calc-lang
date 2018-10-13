using System;

namespace CalcLang.CodeAnalysis {
    public sealed class Diagnostic {
        internal Diagnostic( Location location, string message ) {
            if ( string.IsNullOrWhiteSpace( message ) ) {
                throw new ArgumentException( "message", nameof( message ) );
            }

            Location = location;
            Message = message;
        }

        public Location Location { get; }
        public string Message { get; }
    }
}
