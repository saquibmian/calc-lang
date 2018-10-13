using System.Linq;
using Xunit;

namespace CalcLang.CodeAnalysis.Syntax {
    public sealed class ParserTests {

        [Fact]
        public void Integer__Valid__ParsesNumberExpression() {
            const string input = "1234";

            var expr = ParseExpression<IntegerLiteralExpressionSyntax>( input );

            Assert.Equal( int.Parse( input ), expr.NumberToken.Value );
        }

        [Theory]
        [InlineData( "1234.1" )]
        [InlineData( "1234f" )]
        public void Float__Valid__ParsesFloatExpression( string input ) {
            var expr = ParseExpression<FloatLiteralExpressionSyntax>( input );

            Assert.Equal( float.Parse( input.TrimEnd( 'f' ) ), expr.NumberToken.Value );
        }

        [Theory]
        [InlineData( "true" )]
        [InlineData( "false" )]
        public void Boolean__Valid__ParsesBooleanLiteralExpression( string input ) {
            var expr = ParseExpression<BooleanLiteralExpressionSyntax>( input );

            Assert.Equal( bool.Parse( input ), expr.Value );
        }

        [Theory]
        [InlineData( "foo" )]
        [InlineData( "m_something" )]
        [InlineData( "_something" )]
        public void Identifier__Valid__ParsesMemberAccessExpression( string input ) {
            var expr = ParseExpression<MemberAccessExpressionSyntax>( input );

            Assert.Equal( input, expr.MemberName.ValueText );
        }

        [Theory]
        [InlineData( "1 + 2", SyntaxKind.PlusToken )]
        [InlineData( "1 - 2", SyntaxKind.MinusToken )]
        [InlineData( "1 * 2", SyntaxKind.StarToken )]
        [InlineData( "1 / 2", SyntaxKind.ForwardSlashToken )]
        public void Operator__Valid__ParsesBinaryExpression( string input, SyntaxKind operatorTokenKind ) {
            var expr = ParseExpression<BinaryExpressionSyntax>( input );

            Assert.Equal( operatorTokenKind, expr.OperatorToken.Kind );
            var left = Assert.IsType<IntegerLiteralExpressionSyntax>( expr.Left );
            Assert.Equal( 1, left.NumberToken.Value );
            var right = Assert.IsType<IntegerLiteralExpressionSyntax>( expr.Right );
            Assert.Equal( 2, right.NumberToken.Value );
        }

        [Theory]
        [InlineData( "+1", SyntaxKind.PlusToken )]
        [InlineData( "-1", SyntaxKind.MinusToken )]
        public void Operator__Valid__ParsesUnaryExpression( string input, SyntaxKind operatorTokenKind ) {
            var expr = ParseExpression<UnaryExpressionSyntax>( input );

            Assert.Equal( operatorTokenKind, expr.OperatorToken.Kind );
            var left = Assert.IsType<IntegerLiteralExpressionSyntax>( expr.Expression );
            Assert.Equal( 1, left.NumberToken.Value );
        }

        [Fact]
        public void Invocation__Valid__ParsesInvocationExpression() {
            const string input = "foo()";

            var expr = ParseExpression<InvocationExpressionSyntax>( input );

            Assert.Equal( "foo", expr.Member.MemberName.ValueText );
            Assert.Empty( expr.ArgumentList.Arguments.Nodes );
        }

        [Fact]
        public void Invocation__Valid__ParsesArgumentsCorrectly() {
            const string input = "foo(1, 2, 2.5, _bar, (1+2 ) )";

            var expr = ParseExpression<InvocationExpressionSyntax>( input );

            var args = expr.ArgumentList.Arguments.Nodes.ToArray();
            var firstArg = Assert.IsType<IntegerLiteralExpressionSyntax>( args[0].Expression );
            Assert.Equal( 1, firstArg.NumberToken.Value );
            var secondArg = Assert.IsType<IntegerLiteralExpressionSyntax>( args[1].Expression );
            Assert.Equal( 2, secondArg.NumberToken.Value );
            var thirdArg = Assert.IsType<FloatLiteralExpressionSyntax>( args[2].Expression );
            Assert.Equal( 2.5f, thirdArg.NumberToken.Value );
            var fourthArg = Assert.IsType<MemberAccessExpressionSyntax>( args[3].Expression );
            Assert.Equal( "_bar", fourthArg.MemberName.ValueText );
            var fifthArg = Assert.IsType<ParenthetizedExpressionSyntax>( args[4].Expression );
            var fifthArgExpression = Assert.IsType<BinaryExpressionSyntax>( fifthArg.Expression );
        }

        private T ParseExpression<T>( string input ) where T : ExpressionSyntax {
            var tree = SyntaxTree.Parse( input );
            Assert.Empty( tree.Diagnostics );
            return Assert.IsType<T>( tree.Root );
        }

    }
}
