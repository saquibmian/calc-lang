namespace CalcLang.CodeAnalysis {
    internal class SyntaxToken {
        public SyntaxToken( SyntaxKind kind, int position, string text, object value ) {
            Kind = kind;
            Position = position;
            Text = text;
            Value = value;
        }

        internal SyntaxKind Kind { get; }
        public int Position { get; }
        public string Text { get; }
        public object Value { get; }
    }
}
