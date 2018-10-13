using CalcLang.CodeAnalysis.Binding;
using CalcLang.CodeAnalysis.Syntax;
using Xunit;

namespace CalcLang {
    public sealed class ExpressionEvaluatorTests {

        private readonly Binder _binder = new Binder();
        private readonly ExpressionEvaluator _evaluator = new ExpressionEvaluator();

        [Fact]
        public void IntegerLiteral__Works() {
            var expr = Expression<BoundLiteralExpression>( "1" );

            var result = _evaluator.Evaluate( expr );

            Assert.Equal( 1, result );
        }

        [Fact]
        public void FloatLiteral__Works() {
            var expr = Expression<BoundLiteralExpression>( "1.1f" );

            var result = _evaluator.Evaluate( expr );

            Assert.Equal( 1.1f, result );
        }

        [Fact]
        public void Add__Works() {
            var expr = Expression<BoundBinaryExpression>( "1 + 2" );

            var result = _evaluator.Evaluate( expr );

            Assert.Equal( 3, result );
        }

        [Theory]
        [InlineData( "+1", 1 )]
        [InlineData( "-1", -1 )]
        [InlineData( "-2 * 3", -2 * 3 )]
        [InlineData( "-(2 * 3 + 1)", -( 2 * 3 + 1 ) )]
        [InlineData( "true", true )]
        [InlineData( "false", false )]
        [InlineData( "!false", true )]
        [InlineData( "false || true", false || true )]
        [InlineData( "false && true", false && true )]
        [InlineData( "1==1", 1 == 1 )]
        [InlineData( "!(1==1)", !( 1 == 1 ) )]
        [InlineData( "true==true", true == true )]
        [InlineData( "!(true==true)", !( true == true ) )]
        [InlineData( "1!=1", 1 != 1 )]
        [InlineData( "!(1!=1)", !( 1 != 1 ) )]
        [InlineData( "true!=true", true != true )]
        [InlineData( "!(true!=true)", !( true != true ) )]
        [InlineData( "1==1&&2==2", 1 == 1 && 2 == 2 )]
        [InlineData( "1==2||2==2", 1 == 2 || 2 == 2 )]
        public void Evaluate( string input, object expectedResult ) {
            var expr = Expression( input );

            var result = _evaluator.Evaluate( expr );

            Assert.Equal( expectedResult, result );
        }

        private BoundExpression Expression( string input ) {
            var tree = SyntaxTree.Parse( input );
            Assert.Empty( tree.Diagnostics );
            var expr = Assert.IsAssignableFrom<ExpressionSyntax>( tree.Root );
            var bound = _binder.BindExpression( expr );
            Assert.Empty( _binder.Diagnostics );
            return bound;
        }

        private T Expression<T>( string input ) where T : BoundExpression {
            var expr = Expression( input );
            return Assert.IsType<T>( expr );
        }
    }
}