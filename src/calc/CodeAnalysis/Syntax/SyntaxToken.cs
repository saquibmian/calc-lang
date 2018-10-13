namespace CalcLang.CodeAnalysis.Syntax {
    public sealed class SyntaxToken {
        internal SyntaxToken( SyntaxKind kind, Location location, string text, object value ) {
            Kind = kind;
            Location = location;
            ValueText = text;
            Value = value;
        }

        public SyntaxKind Kind { get; }
        public Location Location { get; }
        public string ValueText { get; }
        public object Value { get; }
    }
}
