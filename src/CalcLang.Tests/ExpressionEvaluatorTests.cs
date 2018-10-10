using System;
using System.Collections.Immutable;
using CalcLang.CodeAnalysis;
using Xunit;

namespace CalcLang {
    public sealed class ExpressionEvaluatorTests {

        private readonly ExpressionEvaluator _evaluator = new ExpressionEvaluator();
        private readonly Runtime _runtime = Runtime.Global.CreateScope();

        public ExpressionEvaluatorTests() {
            _runtime.AddMethod( new EchoMethod() );
        }

        [Fact]
        public void IntegerLiteral__Works() {
            var expr = Expression<IntegerLiteralExpressionSyntax>( "1" );

            var result = _evaluator.Evaluate( expr, _runtime );

            Assert.Equal( 1, result );
        }

        [Fact]
        public void FloatLiteral__Works() {
            var expr = Expression<FloatLiteralExpressionSyntax>( "1.1" );

            var result = _evaluator.Evaluate( expr, _runtime );

            Assert.Equal( 1.1f, result );
        }

        [Fact]
        public void MemberAccess__Works() {
            var expr = Expression<MemberAccessExpressionSyntax>( "foo" );
            _runtime.SetVariable( "foo", 1 );

            var result = _evaluator.Evaluate( expr, _runtime );

            Assert.Equal( 1, result );
        }

        [Fact]
        public void Add__Works() {
            var expr = Expression<BinaryExpressionSyntax>( "1 + foo" );
            _runtime.SetVariable( "foo", 1 );

            var result = _evaluator.Evaluate( expr, _runtime );

            Assert.Equal( 2, result );
        }

        [Fact]
        public void Invocation__Works() {
            var expr = Expression<InvocationExpressionSyntax>( "echo(99)" );

            var result = _evaluator.Evaluate( expr, _runtime );

            Assert.Equal( 99, result );
        }

        [Fact]
        public void SomethingComplexBecauseImLazy() {
            var expr = Expression<BinaryExpressionSyntax>( "1 + (echo((99+2+foo)) + foo)" );
            _runtime.SetVariable( "foo", 100 );

            var result = _evaluator.Evaluate( expr, _runtime );

            Assert.Equal( ( 1 + 99 + 2 + 100 + 100 ), result );
        }

        private T Expression<T>( string input ) where T : ExpressionSyntax {
            var tree = Parser.Parse( input );
            var expr = Assert.IsType<ExpressionStatementSyntax>( tree.Root );
            return Assert.IsType<T>( expr.Expression );
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