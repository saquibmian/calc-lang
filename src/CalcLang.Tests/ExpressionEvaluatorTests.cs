using System;
using System.Collections.Immutable;
using CalcLang.CodeAnalysis;
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
            var expr = Expression<BoundLiteralExpression>( "1.1" );

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
        [InlineData( "false || true", true )]
        [InlineData( "false && true", false )]
        [InlineData( "1==1", true )]
        [InlineData( "!(1==1)", false )]
        [InlineData( "true==true", true )]
        [InlineData( "!(true==true)", false )]
        [InlineData( "1!=1", false )]
        [InlineData( "!(1!=1)", true )]
        [InlineData( "true!=true", false )]
        [InlineData( "!(true!=true)", true )]
        [InlineData( "1==1&&2==2", true )]
        [InlineData( "1==2||2==2", true )]
        public void Evaluate( string input, object expectedResult ) {
            var tree = SyntaxTree.Parse( input );
            Assert.Empty( tree.Diagnostics );
            var expr = Assert.IsAssignableFrom<ExpressionSyntax>( tree.Root );
            var bound = _binder.BindExpression( expr );
            Assert.Empty( _binder.Diagnostics );

            var result = _evaluator.Evaluate( bound );

            Assert.Equal( expectedResult, result );
        }

        private T Expression<T>( string input ) where T : BoundExpression {
            var tree = SyntaxTree.Parse( input );
            var expr = Assert.IsAssignableFrom<ExpressionSyntax>( tree.Root );
            var bound = _binder.BindExpression( expr );
            return Assert.IsType<T>( bound );
        }
    }
}