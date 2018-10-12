using System.Linq;
using Xunit;

namespace CalcLang.CodeAnalysis {
    public sealed class LexerTests {

        [Theory]
        [InlineData( "+", SyntaxKind.PlusToken )]
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
        [InlineData( "1234" )]
        [InlineData( "123422222" )]
        public void Integer__Valid__LexesIntegerToken( string input ) {
            var result = LexSingle( input );

            Assert.Equal( SyntaxKind.IntegerToken, result.Kind );
            Assert.Equal( input, result.ValueText );
            Assert.Equal( int.Parse( input ), result.Value );
        }

        [Theory]
        [InlineData( "1f" )]
        [InlineData( "1234.1" )]
        [InlineData( "123422222.1" )]
        public void Float__Valid__LexesFloatToken( string input ) {
            var result = LexSingle( input );

            Assert.Equal( SyntaxKind.FloatToken, result.Kind );
            Assert.Equal( input, result.ValueText );
            Assert.Equal( float.Parse( input.TrimEnd( 'f' ) ), result.Value );
        }

        [Theory]
        [InlineData( "foo" )]
        [InlineData( "m_something" )]
        [InlineData( "_something" )]
        [InlineData( "_something_" )]
        [InlineData( "_something123" )]
        public void Identifier__Valid__LexesIdentifierToken( string input ) {
            var result = LexSingle( input );

            Assert.Equal( SyntaxKind.IdentiferToken, result.Kind );
            Assert.Equal( input, result.ValueText );
            Assert.Equal( input, result.Value );
        }

        [Theory]
        [InlineData( "1a" )]
        public void Identifier__StartsWithDigit__LexesFloatTokenWithDiag( string input ) {
            var result = LexSingle( input, errorCount: 1 );

            Assert.Equal( SyntaxKind.FloatToken, result.Kind );
            Assert.Equal( input, result.ValueText );
            Assert.Null( result.Value );
        }

        private SyntaxToken LexSingle( string input, int errorCount = 0 ) {
            var lexer = new Lexer( input );
            var token = lexer.Lex();
            Assert.Equal( errorCount, lexer.Diagnostics.Count() );
            return token;
        }

    }
}
