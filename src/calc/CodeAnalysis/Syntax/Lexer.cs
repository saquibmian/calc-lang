using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace CalcLang.CodeAnalysis.Syntax {
    internal sealed class Lexer {
        private readonly SlidingTextWindow _window;
        private readonly List<Diagnostic> _diagnostics = new List<Diagnostic>();

        public Lexer( string input ) {
            _window = new SlidingTextWindow( input ?? throw new System.ArgumentNullException( nameof( input ) ) );
        }

        public IEnumerable<Diagnostic> Diagnostics => _diagnostics;

        public SyntaxToken Lex() {
            _window.Start();

            switch ( _window.Peek() ) {

                // end of file
                case '\0':
                    return new SyntaxToken( SyntaxKind.EndOfFileToken, _window.WindowStart, "\0", null );

                // one-character tokens
                case '+':
                    _window.Next();
                    return new SyntaxToken( SyntaxKind.PlusToken, _window.WindowStart, "+", null );
                case '-':
                    _window.Next();
                    return new SyntaxToken( SyntaxKind.MinusToken, _window.WindowStart, "-", null );
                case '/':
                    _window.Next();
                    return new SyntaxToken( SyntaxKind.ForwardSlashToken, _window.WindowStart, "/", null );
                case '*':
                    _window.Next();
                    return new SyntaxToken( SyntaxKind.StarToken, _window.WindowStart, "*", null );
                case '(':
                    _window.Next();
                    return new SyntaxToken( SyntaxKind.OpenParenthesisToken, _window.WindowStart, "(", null );
                case ')':
                    _window.Next();
                    return new SyntaxToken( SyntaxKind.CloseParenthesisToken, _window.WindowStart, ")", null );
                case ',':
                    _window.Next();
                    return new SyntaxToken( SyntaxKind.CommaToken, _window.WindowStart, ",", null );
                case '!':
                    _window.Next();
                    return new SyntaxToken( SyntaxKind.BangToken, _window.WindowStart, "!", null );
                case '=':
                    _window.Next();
                    if ( _window.Peek() == '=' ) {
                        _window.Next();
                        return new SyntaxToken( SyntaxKind.EqualsEqualsToken, _window.WindowStart, "==", null );
                    }
                    return new SyntaxToken( SyntaxKind.EqualsToken, _window.WindowStart, "=", null );
                case '&':
                    _window.Next();
                    if ( _window.Peek() == '&' ) {
                        _window.Next();
                        return new SyntaxToken( SyntaxKind.AmpersandAmpersandToken, _window.WindowStart, "&&", null );
                    }
                    return new SyntaxToken( SyntaxKind.AmpersandToken, _window.WindowStart, "&", null );
                case '|':
                    _window.Next();
                    if ( _window.Peek() == '|' ) {
                        _window.Next();
                        return new SyntaxToken( SyntaxKind.PipePipeToken, _window.WindowStart, "||", null );
                    }
                    return new SyntaxToken( SyntaxKind.PipeToken, _window.WindowStart, "|", null );

                // numbers
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                    return LexNumber();

                // whitespace
                case var ws when char.IsWhiteSpace( ws ):
                    while ( char.IsWhiteSpace( _window.Peek() ) ) {
                        _window.Next();
                    }
                    var whitespace = _window.Value;
                    return new SyntaxToken( SyntaxKind.WhiteSpaceToken, _window.WindowStart, whitespace, null );

                // words
                case var letter when char.IsLetter( letter ) || letter == '_':
                    var identifer = ReadIdentifier();
                    var kind = SyntaxFacts.GetKeywordKind( identifer );
                    return new SyntaxToken( kind, _window.WindowStart, identifer, null );

                // bad token
                default:
                    var badToken = ReadUntilWhitespace();
                    _diagnostics.Add( new Diagnostic( _window.WindowStart, $"Bad character input: '{badToken}'" ) );
                    return new SyntaxToken( SyntaxKind.BadToken, _window.WindowStart, badToken, null );

            }
        }

        private SyntaxToken LexNumber() {
            while ( char.IsDigit( _window.Peek() ) ) {
                _window.Next();
            }

            float parsedFloat;
            switch ( _window.Peek() ) {
                case 'f':
                    _window.Next();
                    if ( !float.TryParse( _window.Value.TrimEnd( 'f' ), out parsedFloat ) ) {
                        _diagnostics.Add( new Diagnostic( _window.WindowStart, $"Expected Float32, but found '{_window.Value}'" ) );
                    }
                    return new SyntaxToken( SyntaxKind.FloatToken, _window.WindowStart, _window.Value, parsedFloat );

                case '.':
                    _window.Next();
                    while ( char.IsDigit( _window.Peek() ) ) {
                        _window.Next();
                    }
                    if ( !float.TryParse( _window.Value, out parsedFloat ) ) {
                        _diagnostics.Add( new Diagnostic( _window.WindowStart, $"Expected Float32, but found '{_window.Value}'" ) );
                    }
                    return new SyntaxToken( SyntaxKind.FloatToken, _window.WindowStart, _window.Value, parsedFloat );

                case var letter when char.IsLetter( letter ):
                    _window.Next();
                    _diagnostics.Add( new Diagnostic( _window.WindowStart, $"Expected Int32, but found '{_window.Value}'" ) );
                    return new SyntaxToken( SyntaxKind.IntegerToken, _window.WindowStart, _window.Value, null );

                default:
                    if ( !int.TryParse( _window.Value, out var parsedInteger ) ) {
                        _diagnostics.Add( new Diagnostic( _window.WindowStart, $"Expected Int32, but found '{_window.Value}'" ) );
                    }
                    return new SyntaxToken( SyntaxKind.IntegerToken, _window.WindowStart, _window.Value, parsedInteger );
            }
        }

        private string ReadIdentifier() {
            while ( char.IsLetterOrDigit( _window.Peek() ) || _window.Peek() == '_' ) {
                _window.Next();
            }
            return _window.Value;
        }

        private string ReadUntilWhitespace() {
            while ( !char.IsWhiteSpace( _window.Peek() ) ) {
                _window.Next();
            }
            return _window.Value;
        }

        private string ReadNumber() {
            while ( char.IsDigit( _window.Peek() ) ) {
                _window.Next();
            }
            return _window.Value;
        }

    }
}
