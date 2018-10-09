namespace CalcLang.CodeAnalysis {
    public sealed class SyntaxToken {
        internal SyntaxToken( SyntaxKind kind, int position, string text, object value ) {
            Kind = kind;
            Position = position;
            ValueText = text;
            Value = value;
        }

        public SyntaxKind Kind { get; }
        public int Position { get; }
        public string ValueText { get; }
        public object Value { get; }
    }
}
