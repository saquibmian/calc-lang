using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace CalcLang.CodeAnalysis.Syntax {
    public sealed class LexerTests {

        public static IEnumerable<string> Identifiers {
            get {
                yield return "foo";
                yield return "m_something";
                yield return "_something";
                yield return "_something_";
                yield return "_something123";
            }
        }

        public static IEnumerable<string> Integers {
            get {
                yield return "1234";
                yield return "123422222";
            }
        }

        public static IEnumerable<string> Floats {
            get {
                yield return "1f";
                yield return "1234.1";
                yield return "123422222.1";
            }
        }

        public static IEnumerable<(string Text, SyntaxKind Kind)> Keywords {
            get {
                yield return ("true", SyntaxKind.TrueKeyword);
                yield return ("false", SyntaxKind.FalseKeyword);
            }
        }

        public static IEnumerable<(string Text, SyntaxKind Kind)> Operators {
            get {
                yield return ("+", SyntaxKind.PlusToken);
                yield return ("-", SyntaxKind.MinusToken);
                yield return ("*", SyntaxKind.StarToken);
                yield return ("/", SyntaxKind.ForwardSlashToken);
                yield return ("!", SyntaxKind.BangToken);
                yield return ("&", SyntaxKind.AmpersandToken);
                yield return ("&&", SyntaxKind.AmpersandAmpersandToken);
                yield return ("|", SyntaxKind.PipeToken);
                yield return ("||", SyntaxKind.PipePipeToken);
                yield return ("=", SyntaxKind.EqualsToken);
                yield return ("==", SyntaxKind.EqualsEqualsToken);
                yield return ("!=", SyntaxKind.BangEqualsToken);
            }
        }

        public static IEnumerable<object[]> IdentifiersData => Identifiers.Select( i => new[] { i } );
        public static IEnumerable<object[]> IntegersData => Integers.Select( i => new[] { i } );
        public static IEnumerable<object[]> FloatsData => Floats.Select( i => new[] { i } );
        public static IEnumerable<object[]> KeywordsData => Keywords.Select( i => new object[] { i.Text, i.Kind } );
        public static IEnumerable<object[]> OperatorsData => Operators.Select( i => new object[] { i.Text, i.Kind } );
        public static IEnumerable<object[]> PairwiseData {
            get {
                foreach ( var op in Operators ) {
                    foreach ( var identifier in Identifiers ) {
                        yield return new object[] { op.Text + identifier, (op.Text, op.Kind), (identifier, SyntaxKind.IdentiferToken) };
                        yield return new object[] { identifier + op.Text, (identifier, SyntaxKind.IdentiferToken), (op.Text, op.Kind) };
                    }
                    foreach ( var floatText in Floats ) {
                        yield return new object[] { op.Text + floatText, (op.Text, op.Kind), (floatText, SyntaxKind.FloatToken) };
                        yield return new object[] { floatText + op.Text, (floatText, SyntaxKind.FloatToken), (op.Text, op.Kind) };
                    }
                    foreach ( var integerText in Integers ) {
                        yield return new object[] { op.Text + integerText, (op.Text, op.Kind), (integerText, SyntaxKind.IntegerToken) };
                        yield return new object[] { integerText + op.Text, (integerText, SyntaxKind.IntegerToken), (op.Text, op.Kind) };
                    }
                }
                foreach ( var identifier in Identifiers ) {
                    foreach ( var secondIdentifier in Identifiers ) {
                        yield return new object[] { identifier + " " + secondIdentifier, (identifier, SyntaxKind.IdentiferToken), (secondIdentifier, SyntaxKind.IdentiferToken) };
                        yield return new object[] { secondIdentifier + " " + identifier, (secondIdentifier, SyntaxKind.IdentiferToken), (identifier, SyntaxKind.IdentiferToken) };
                    }
                    foreach ( var keyword in Keywords ) {
                        yield return new object[] { identifier + " " + keyword.Text, (identifier, SyntaxKind.IdentiferToken), (keyword.Text, keyword.Kind) };
                        yield return new object[] { keyword.Text + " " + identifier, (keyword.Text, keyword.Kind), (identifier, SyntaxKind.IdentiferToken) };
                    }
                    foreach ( var integer in Integers ) {
                        yield return new object[] { identifier + " " + integer, (identifier, SyntaxKind.IdentiferToken), (integer, SyntaxKind.IntegerToken) };
                        yield return new object[] { integer + " " + identifier, (integer, SyntaxKind.IntegerToken), (identifier, SyntaxKind.IdentiferToken) };
                    }
                    foreach ( var floatText in Floats ) {
                        yield return new object[] { identifier + " " + floatText, (identifier, SyntaxKind.IdentiferToken), (floatText, SyntaxKind.FloatToken) };
                        yield return new object[] { floatText + " " + identifier, (floatText, SyntaxKind.FloatToken), (identifier, SyntaxKind.IdentiferToken) };
                    }
                }
            }
        }

        [Theory]
        [MemberData( nameof( OperatorsData ) )]
        public void Operator__Valid__LexesCorrectOperatorToken( string input, SyntaxKind expectedKind ) {
            var result = LexSingle( input );

            Assert.Equal( expectedKind, result.Kind );
            Assert.Equal( input, result.ValueText );
            Assert.Null( result.Value );
        }

        [Theory]
        [InlineData( "    " )]
        public void WhiteSpace__Valid__LexesWhiteSpaceToken( string input ) {
            var result = LexSingle( input );

            Assert.Equal( SyntaxKind.WhiteSpaceToken, result.Kind );
            Assert.Equal( input, result.ValueText );
            Assert.Null( result.Value );
        }

        [Theory]
        [MemberData( nameof( IntegersData ) )]
        public void Integer__Valid__LexesIntegerToken( string input ) {
            var result = LexSingle( input );

            Assert.Equal( SyntaxKind.IntegerToken, result.Kind );
            Assert.Equal( input, result.ValueText );
            Assert.Equal( int.Parse( input ), result.Value );
        }

        [Theory]
        [MemberData( nameof( FloatsData ) )]
        public void Float__Valid__LexesFloatToken( string input ) {
            var result = LexSingle( input );

            Assert.Equal( SyntaxKind.FloatToken, result.Kind );
            Assert.Equal( input, result.ValueText );
            Assert.Equal( float.Parse( input.TrimEnd( 'f' ) ), result.Value );
        }

        [Theory]
        [MemberData( nameof( KeywordsData ) )]
        public void Keyword__Valid__LexesKeywordToken( string input, SyntaxKind expectedKind ) {
            var result = LexSingle( input );

            Assert.Equal( expectedKind, result.Kind );
            Assert.Equal( input, result.ValueText );
            Assert.Null( result.Value );
        }

        [Theory]
        [MemberData( nameof( IdentifiersData ) )]
        public void Identifier__Valid__LexesIdentifierToken( string input ) {
            var result = LexSingle( input );

            Assert.Equal( SyntaxKind.IdentiferToken, result.Kind );
            Assert.Equal( input, result.ValueText );
            Assert.Null( result.Value );
        }

        [Theory]
        [MemberData( nameof( PairwiseData ) )]
        public void Pairwise__PairOrTokens__LexesCorrectly( string input, (string Text, SyntaxKind Kind) first, (string Text, SyntaxKind Kind) second ) {
            var result = LexPair( input );

            Assert.Equal( first.Text, result.First.ValueText );
            Assert.Equal( first.Kind, result.First.Kind );
            Assert.Equal( second.Text, result.Second.ValueText );
            Assert.Equal( second.Kind, result.Second.Kind );
        }

        [Theory]
        [InlineData( "1a" )]
        public void Identifier__StartsWithDigit__LexesIntTokenWithDiag( string input ) {
            var result = LexSingle( input, errorCount: 1 );

            Assert.Equal( SyntaxKind.IntegerToken, result.Kind );
            Assert.Equal( input, result.ValueText );
            Assert.Null( result.Value );
        }

        private SyntaxToken LexSingle( string input, int errorCount = 0 ) {
            var lexer = new Lexer( input );
            var token = lexer.Lex();
            Assert.Equal( errorCount, lexer.Diagnostics.Count() );
            return token;
        }

        private (SyntaxToken First, SyntaxToken Second) LexPair( string input, int errorCount = 0 ) {
            var lexer = new Lexer( input );

            var first = lexer.Lex();
            var second = lexer.Lex();
            if ( second.Kind == SyntaxKind.WhiteSpaceToken ) {
                second = lexer.Lex();
            }

            Assert.Equal( errorCount, lexer.Diagnostics.Count() );

            return (first, second);
        }

    }
}
