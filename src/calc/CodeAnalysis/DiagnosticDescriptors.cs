namespace CalcLang.CodeAnalysis {
    public static class DiagnosticDescriptors {
        public static DiagnosticDescriptor UnexpectedToken { get; } = DiagnosticDescriptor.Create( 
            "CL0001", 
            DiagnosticSeverity.Error,
            "Unexpected token <{0}>, expected <{1}>." 
        );

        public static DiagnosticDescriptor BadCharacterInput { get; } = DiagnosticDescriptor.Create( 
            "CL0002", 
            DiagnosticSeverity.Error,
            "Bad character input: '{0}'." 
        );

        public static DiagnosticDescriptor UnexpectedLiteralType { get; } = DiagnosticDescriptor.Create( 
            "CL0003", 
            DiagnosticSeverity.Error,
            "Expected literal of type {0}, but found '{1}'." 
        ); 
        
        public static DiagnosticDescriptor UndefinedBinaryOperator { get; } = DiagnosticDescriptor.Create( 
            "CL0004", 
            DiagnosticSeverity.Error,
            "Binary operator '{0}' is not defined for types {1} and {2}." 
        ); 

        public static DiagnosticDescriptor UndefinedUnaryOperator { get; } = DiagnosticDescriptor.Create( 
            "CL0005", 
            DiagnosticSeverity.Error,
            "Unary operator '{0}' is not defined for type {1}." 
        ); 

    }
}
