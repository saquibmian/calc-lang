using System.Collections.Generic;
using System.Linq;

namespace CalcLang.CodeAnalysis.Syntax {
    internal sealed class SlidingTextWindow {
        private readonly string _text;
        private int _position;
        private int _offset;
        private int _line = 0;
        private int _col = 0;


        public SlidingTextWindow( string text ) {
            _text = text ?? throw new System.ArgumentNullException( nameof( text ) );
        }

        internal Location Location => new Location( _position, _line, _col );

        internal void Start() {
            _position = _position + _offset;
            _offset = 0;

            int indexOfLine = 0;
            for ( int i = 0; i < _position; ++i ) {
                if ( _text[i] == '\n' ) {
                    ++_line;
                    indexOfLine = i;
                }
            }
            _col = _position - indexOfLine;
        }

        internal void Next() => ++_offset;

        internal char Peek( int offset = 0 ) {
            var index = _position + _offset + offset;
            if ( index >= _text.Length ) {
                return '\0';
            }
            return _text[index];
        }

        internal string Value => _text.Substring( _position, _offset );
    }
}
