using Xunit;

namespace CalcLang.CodeAnalysis {
    public sealed class ParserTests {

        [Theory]
        [InlineData( "1234" )]
        [InlineData( "123422222" )]
        public void Integer__Valid__ParsesNumberExpression( string input ) {
            var result = ParseExpression( input );

            var number = Assert.IsType<IntegerLiteralExpressionSyntax>( result.Expression );
            Assert.Equal( int.Parse( input ), number.NumberToken.Value );
        }

        [Theory]
        [InlineData( "1234.1" )]
        [InlineData( "123422222.1" )]
        public void Float__Valid__ParsesFloatExpression( string input ) {
            var result = ParseExpression( input );

            var number = Assert.IsType<FloatLiteralExpressionSyntax>( result.Expression );
            Assert.Equal( float.Parse( input ), number.NumberToken.Value );
        }

        [Theory]
        [InlineData( "foo" )]
        [InlineData( "m_something" )]
        [InlineData( "_something" )]
        public void Identifier__Valid__ParsesMemberAccessExpression( string input ) {
            var result = ParseExpression( input );

            var member = Assert.IsType<MemberAccessExpressionSyntax>( result.Expression );
            Assert.Equal( input, member.MemberName.Value );
        }

        private ExpressionStatementSyntax ParseExpression( string input ) {
            var result = Parser.Parse( input );
            return Assert.IsType<ExpressionStatementSyntax>( result.Root );
        }

    }
}
