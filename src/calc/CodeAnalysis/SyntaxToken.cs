namespace CalcLang.CodeAnalysis {
    internal sealed class SyntaxToken {
        public SyntaxToken( SyntaxKind kind, int position, string text, object value ) {
            Kind = kind;
            Position = position;
            ValueText = text;
            Value = value;
        }

        internal SyntaxKind Kind { get; }
        public int Position { get; }
        public string ValueText { get; }
        public object Value { get; }
    }
}
