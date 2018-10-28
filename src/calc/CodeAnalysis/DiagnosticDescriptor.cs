using System;

namespace CalcLang.CodeAnalysis {
    public sealed class DiagnosticDescriptor {
        private DiagnosticDescriptor( string id, DiagnosticSeverity severity, string messageFormat ) {
            if ( string.IsNullOrWhiteSpace( id ) ) {
                throw new ArgumentException( nameof( id ) );
            }
            if ( string.IsNullOrWhiteSpace( messageFormat ) ) {
                throw new ArgumentException( nameof( messageFormat ) );
            }

            Id = id;
            Severity = severity;
            MessageFormat = messageFormat;
        }

        public string Id { get; }
        public DiagnosticSeverity Severity { get; }
        public string MessageFormat { get; }

        public static DiagnosticDescriptor Create(
            string id,
            DiagnosticSeverity severity,
            string messageFormat
        ) => new DiagnosticDescriptor( id, severity, messageFormat );
    }
}
