namespace CalcLang.CodeAnalysis {
    public sealed class Location {
        internal Location( int position, int length ) {
            Position = position;
            Length = length;
        }

        public int Position { get; }
        public int Length { get; }

        public override string ToString() {
            return $"({Position})";
        }
    }
}
