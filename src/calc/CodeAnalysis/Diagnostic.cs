using System;

namespace CalcLang.CodeAnalysis {
    public sealed class Diagnostic {
        private Diagnostic( string id, Location location, string message ) {
            if ( string.IsNullOrWhiteSpace( id ) ) {
                throw new ArgumentException( nameof( id ) );
            }
            if ( string.IsNullOrWhiteSpace( message ) ) {
                throw new ArgumentException( nameof( message ) );
            }

            Id = id;
            Location = location;
            Message = message;
        }

        public string Id { get; }
        public Location Location { get; }
        public string Message { get; }

        public static Diagnostic Create(
            DiagnosticDescriptor descriptor,
            Location location,
            params object[] messageArguments
        ) => new Diagnostic(
            descriptor.Id,
            location,
            string.Format( descriptor.MessageFormat, messageArguments )
        );
    }
}
