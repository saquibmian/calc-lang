using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace CalcLang.CodeAnalysis {
    internal sealed class Lexer {
        private readonly string _input;
        private readonly List<Diagnostic> _diagnostics = new List<Diagnostic>();

        private int _position = 0;

        internal Lexer( string input ) {
            _input = input ?? throw new System.ArgumentNullException( nameof( input ) );
        }

        internal IEnumerable<Diagnostic> Diagnostics => _diagnostics;

        internal SyntaxToken Read() {
            return ReadNextToken();
        }

        private (int Position, char Character) Current {
            get {
                if ( _position >= _input.Length ) {
                    return (_input.Length, '\0');
                }
                return (_position, _input[_position]);
            }
        }

        private void Next() => ++_position;

        private SyntaxToken ReadNextToken() {
            var start = Current.Position;

            switch ( Current.Character ) {

                // end of file
                case '\0':
                    return new SyntaxToken( SyntaxKind.EndOfFileToken, start, "\0", null );

                // one-character tokens
                case '+':
                    Next();
                    return new SyntaxToken( SyntaxKind.PlusToken, start, "+", null );
                case '-':
                    Next();
                    return new SyntaxToken( SyntaxKind.MinusToken, start, "-", null );
                case '/':
                    Next();
                    return new SyntaxToken( SyntaxKind.ForwardSlashToken, start, "/", null );
                case '*':
                    Next();
                    return new SyntaxToken( SyntaxKind.StarToken, start, "*", null );
                case '(':
                    Next();
                    return new SyntaxToken( SyntaxKind.OpenParenthesisToken, start, "(", null );
                case ')':
                    Next();
                    return new SyntaxToken( SyntaxKind.CloseParenthesisToken, start, ")", null );
                case ',':
                    Next();
                    return new SyntaxToken( SyntaxKind.CommaToken, start, ",", null );
                case '=':
                    Next();
                    return new SyntaxToken( SyntaxKind.EqualsToken, start, "=", null );

                // whitespace
                case var ws when char.IsWhiteSpace( ws ):
                    while ( char.IsWhiteSpace( Current.Character ) ) {
                        Next();
                    }
                    var whitespace = _input.Substring( start, Current.Position - start );
                    return new SyntaxToken( SyntaxKind.WhiteSpaceToken, start, whitespace, null );

                // numbers
                case var digit when char.IsDigit( digit ):
                    var toParse = ReadNumber();
                    if ( Current.Character == '.' ) {
                        Next();
                        toParse = toParse + "." + ReadNumber();
                        if ( !float.TryParse( toParse, out var parsedFloat ) ) {
                            _diagnostics.Add( new Diagnostic( start, $"Expected Float32, but found '{toParse}'" ) );
                        }
                        return new SyntaxToken( SyntaxKind.FloatToken, start, toParse, parsedFloat );
                    } else {
                        if ( !int.TryParse( toParse, out var parsedInteger ) ) {
                            _diagnostics.Add( new Diagnostic( start, $"Expected Int32, but found '{toParse}'" ) );
                        }
                        return new SyntaxToken( SyntaxKind.IntegerToken, start, toParse, parsedInteger );
                    }

                // words
                case var letter when char.IsLetter( letter ) || letter == '_':
                    var identifer = ReadIdentifier();
                    return new SyntaxToken( SyntaxKind.IdentiferToken, start, identifer, identifer );

                // bad token
                default:
                    var badToken = ReadIdentifier();
                    _diagnostics.Add( new Diagnostic( start, $"invalid token: '{badToken}'" ) );
                    return new SyntaxToken( SyntaxKind.BadToken, start, badToken, null );

            }
        }

        private string ReadIdentifier() {
            var start = Current.Position;
            while ( char.IsLetterOrDigit( Current.Character ) || Current.Character == '_' ) {
                Next();
            }
            return _input.Substring( start, Current.Position - start );
        }

        private string ReadNumber() {
            var start = Current.Position;
            while ( char.IsDigit( Current.Character ) ) {
                Next();
            }
            return _input.Substring( start, Current.Position - start );
        }

    }
}
