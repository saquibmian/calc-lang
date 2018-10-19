using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace CalcLang.CodeAnalysis.Syntax {
    internal sealed class Lexer {
        private readonly SlidingTextWindow _window;
        private readonly List<Diagnostic> _diagnostics = new List<Diagnostic>();

        public Lexer( string input ) {
            _window = new SlidingTextWindow( input ?? throw new ArgumentNullException( nameof( input ) ) );
        }

        public IEnumerable<Diagnostic> Diagnostics => _diagnostics;

        public SyntaxToken Lex() {
            _window.Start();

            switch ( _window.Peek() ) {

                // end of file
                case '\0':
                    return new SyntaxToken( SyntaxKind.EndOfFileToken, _window.Location, "\0", null );

                // one-character tokens
                case '+':
                    _window.Next();
                    return new SyntaxToken( SyntaxKind.PlusToken, _window.Location, "+", null );
                case '-':
                    _window.Next();
                    return new SyntaxToken( SyntaxKind.MinusToken, _window.Location, "-", null );
                case '/':
                    _window.Next();
                    return new SyntaxToken( SyntaxKind.ForwardSlashToken, _window.Location, "/", null );
                case '*':
                    _window.Next();
                    return new SyntaxToken( SyntaxKind.StarToken, _window.Location, "*", null );
                case '(':
                    _window.Next();
                    return new SyntaxToken( SyntaxKind.OpenParenthesisToken, _window.Location, "(", null );
                case ')':
                    _window.Next();
                    return new SyntaxToken( SyntaxKind.CloseParenthesisToken, _window.Location, ")", null );
                case ',':
                    _window.Next();
                    return new SyntaxToken( SyntaxKind.CommaToken, _window.Location, ",", null );
                case '!':
                    _window.Next();
                    if ( _window.Peek() == '=' ) {
                        _window.Next();
                        return new SyntaxToken( SyntaxKind.BangEqualsToken, _window.Location, "!=", null );
                    }
                    return new SyntaxToken( SyntaxKind.BangToken, _window.Location, "!", null );
                case '=':
                    _window.Next();
                    if ( _window.Peek() == '=' ) {
                        _window.Next();
                        return new SyntaxToken( SyntaxKind.EqualsEqualsToken, _window.Location, "==", null );
                    }
                    return new SyntaxToken( SyntaxKind.EqualsToken, _window.Location, "=", null );
                case '&':
                    _window.Next();
                    if ( _window.Peek() == '&' ) {
                        _window.Next();
                        return new SyntaxToken( SyntaxKind.AmpersandAmpersandToken, _window.Location, "&&", null );
                    }
                    return new SyntaxToken( SyntaxKind.AmpersandToken, _window.Location, "&", null );
                case '|':
                    _window.Next();
                    if ( _window.Peek() == '|' ) {
                        _window.Next();
                        return new SyntaxToken( SyntaxKind.PipePipeToken, _window.Location, "||", null );
                    }
                    return new SyntaxToken( SyntaxKind.PipeToken, _window.Location, "|", null );

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
                    return new SyntaxToken( SyntaxKind.WhiteSpaceToken, _window.Location, whitespace, null );

                // words
                case var letter when char.IsLetter( letter ) || letter == '_':
                    var identifer = ReadIdentifier();
                    var kind = SyntaxFacts.GetKeywordKind( identifer );
                    return new SyntaxToken( kind, _window.Location, identifer, null );

                // bad token
                default:
                    var badToken = ReadUntilWhitespace();
                    _diagnostics.Add( new Diagnostic( _window.Location, $"Bad character input: '{badToken}'" ) );
                    return new SyntaxToken( SyntaxKind.BadToken, _window.Location, badToken, null );

            }
        }

        private SyntaxToken LexNumber() {
            var kind = SyntaxKind.IntegerToken;

            while ( char.IsDigit( _window.Peek() ) ) {
                _window.Next();
            }

        numberKind: switch ( _window.Peek() ) {
                case 'f':
                    kind = SyntaxKind.FloatToken;
                    _window.Next();
                    break;

                case '.':
                    kind = SyntaxKind.FloatToken;
                    _window.Next();
                    while ( char.IsDigit( _window.Peek() ) ) {
                        _window.Next();
                    }
                    goto numberKind;

                case var letter when char.IsLetter( letter ):
                    _window.Next();
                    _diagnostics.Add( new Diagnostic( _window.Location, $"Expected a number, but found '{_window.Value}'" ) );
                    return new SyntaxToken( kind, _window.Location, _window.Value, null );
            }

            switch ( kind ) {
                case SyntaxKind.FloatToken:
                    if ( !float.TryParse( _window.Value.TrimEnd( 'f' ), out float parsedFloat ) ) {
                        _diagnostics.Add( new Diagnostic( _window.Location, $"Expected Float32, but found '{_window.Value}'" ) );
                    }
                    return new SyntaxToken( kind, _window.Location, _window.Value, parsedFloat );

                case SyntaxKind.IntegerToken:
                    if ( !int.TryParse( _window.Value, out int parsedInt ) ) {
                        _diagnostics.Add( new Diagnostic( _window.Location, $"Expected Int32, but found '{_window.Value}'" ) );
                    }
                    return new SyntaxToken( kind, _window.Location, _window.Value, parsedInt );

                default:
                    throw new Exception( $"Unhandled number kind {kind}" );
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
