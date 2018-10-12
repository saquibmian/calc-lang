namespace CalcLang.CodeAnalysis.Syntax {
    internal sealed class SlidingTextWindow {
        private readonly string _text;

        /// <summary>
        /// The start of the current window.
        /// </summary>
        private int _position;

        /// <summary>
        /// The start of the current window.
        /// </summary>
        private int _offset;

        public SlidingTextWindow( string text ) {
            _text = text ?? throw new System.ArgumentNullException( nameof( text ) );
        }

        internal int WindowStart => _position;
        internal int WindowEnd => _position + _offset;

        internal void Start() {
            _position = _position + _offset;
            _offset = 0;
        }

        internal void Next() {
            ++_offset;
        }

        internal char Peek( int offset = 0 ) {
            var index = _position + _offset + offset;
            if ( index >= _text.Length ) {
                return '\0';
            }
            return _text[index];
        }

        internal string Value {
            get {
                return _text.Substring( _position, _offset );
            }
        }
    }
}
