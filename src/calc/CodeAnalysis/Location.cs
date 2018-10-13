namespace CalcLang.CodeAnalysis {
    public sealed class Location {
        internal Location( int position, int line, int column ) {
            Position = position;
            Line = line;
            Column = column;
        }

        public int Position { get; }
        public int Line { get; }
        public int Column { get; }

        public override string ToString() {
            return $"({Line},{Column})";
        }
    }
}
