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
        private readonly Runtime _runtime = Runtime.Global.CreateScope();

        public ExpressionEvaluatorTests() {
            _runtime.AddMethod( new EchoMethod() );
        }

        [Fact]
        public void IntegerLiteral__Works() {
            var expr = Expression<BoundLiteralExpression>( "1" );

            var result = _evaluator.Evaluate( expr, _runtime );

            Assert.Equal( 1, result );
        }

        [Fact]
        public void FloatLiteral__Works() {
            var expr = Expression<BoundLiteralExpression>( "1.1" );

            var result = _evaluator.Evaluate( expr, _runtime );

            Assert.Equal( 1.1f, result );
        }


        [Fact]
        public void Add__Works() {
            var expr = Expression<BoundBinaryExpression>( "1 + 2" );

            var result = _evaluator.Evaluate( expr, _runtime );

            Assert.Equal( 3, result );
        }


        [Theory]
        [InlineData( "+1", 1 )]
        [InlineData( "-1", -1 )]
        [InlineData( "-2 * 3", -2 * 3 )]
        [InlineData( "-(2 * 3 + 1)", -( 2 * 3 + 1 ) )]
        public void TestAllTheThings( string input, int expectedResult ) {
            _runtime.SetVariable( "foo", 100 );
            var tree = SyntaxTree.Parse( input );
            var expr = Assert.IsAssignableFrom<ExpressionSyntax>( tree.Root );
            var bound = _binder.BindExpression( expr ); ;

            var result = _evaluator.Evaluate( bound, _runtime );

            Assert.Equal( expectedResult, result );
        }

        private T Expression<T>( string input ) where T : BoundExpression {
            var tree = SyntaxTree.Parse( input );
            var expr = Assert.IsAssignableFrom<ExpressionSyntax>( tree.Root );
            var bound = _binder.BindExpression( expr );
            return Assert.IsType<T>( bound );
        }

        private sealed class EchoMethod : MethodSymbol {
            public override string Name => "echo";

            public override ImmutableArray<ParameterSymbol> Parameters => ImmutableArray.Create<ParameterSymbol>(
                new ParameterSymbol( "input", typeof( int ) )
            );

            public override Type ReturnType => typeof( int );

            public override object Execute( Runtime runtime ) {
                GetArguments( runtime, out int seed );
                return seed;
            }
        }
    }
}