using System;
using System.Collections.Generic;
using System.Linq;

namespace CalcLang.CodeAnalysis.Syntax {
    internal sealed class SlidingTextWindow {
        private readonly string _text;
        private int _position;
        private int _offset;

        public SlidingTextWindow( string text ) {
            _text = text ?? throw new ArgumentNullException( nameof( text ) );
        }

        internal Location Location => new Location( _position, _offset );

        internal void Start() {
            _position = _position + _offset;
            _offset = 0;
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
