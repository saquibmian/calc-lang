using System;
using System.Collections.Generic;
using System.Linq;
using CalcLang.CodeAnalysis;
using CalcLang.CodeAnalysis.Syntax;

namespace CalcLang {
    public sealed class ExpressionEvaluator {

        public object Evaluate( ExpressionSyntax expression, Runtime runtime ) {
            switch ( expression ) {
                case IntegerLiteralExpressionSyntax n:
                    return n.NumberToken.Value;

                case FloatLiteralExpressionSyntax f:
                    return f.NumberToken.Value;

                case UnaryExpressionSyntax u:
                    return Evaluate( u, runtime );

                case BinaryExpressionSyntax b:
                    return Evaluate( b, runtime );

                case ParenthetizedExpressionSyntax p:
                    return Evaluate( p.Expression, runtime );

                case InvocationExpressionSyntax i:
                    return Evaluate( i, runtime );

                case MemberAccessExpressionSyntax m:
                    return Evaluate( m, runtime );

                default:
                    throw new Exception( $"Unexpected expression {expression.Kind}" );
            }
        }

        private object Evaluate(UnaryExpressionSyntax u, Runtime runtime) {
            // TODO: find a way to determine the return type of the expression
            var expr = Evaluate( u.Expression, runtime );
            var argTypes = new[] { expr.GetType(), expr.GetType() };

            var method = runtime.GetMethod( u.Operand.ValueText, argTypes ); ;
            if ( method == null ) {
                throw new Exception( $"Unknown operator {u.Operand.ValueText}" );
            }

            // create a new scope with the args for the method
            var scope = runtime.CreateScope();
            // for operators, the args are always (lhs, rhs)
            scope.SetVariable( "lhs", 0 );
            scope.SetVariable( "rhs", expr );

            return method.Execute( scope );
        }

        private object Evaluate( BinaryExpressionSyntax b, Runtime runtime ) {
            // TODO: find a way to determine the return type of the expression
            var left = Evaluate( b.Left, runtime );
            var right = Evaluate( b.Right, runtime );
            var argTypes = new[] { left.GetType(), right.GetType() };

            var method = runtime.GetMethod( b.OperatorToken.ValueText, argTypes ); ;
            if ( method == null ) {
                throw new Exception( $"Unknown operator" );
            }

            // create a new scope with the args for the method
            var scope = runtime.CreateScope();
            // for operators, the args are always (lhs, rhs)
            scope.SetVariable( "lhs", left );
            scope.SetVariable( "rhs", right );

            return method.Execute( scope );
        }

        private object Evaluate( InvocationExpressionSyntax i, Runtime runtime ) {
            // TODO: find a way to determine the return type of the expression
            var args = i.ArgumentList.Arguments.Nodes
                .Select( arg => Evaluate( arg.Expression, runtime ) )
                .ToArray();
            var argTypes = args.Select( a => a.GetType() ).ToArray();

            var method = runtime.GetMethod( (string)i.Member.MemberName.Value, argTypes );
            if ( method == null ) {
                throw new Exception( $"Unknown function '{i.Member.MemberName.Value}'" );
            }

            if ( i.ArgumentList.Arguments.Nodes.Count() > method.Parameters.Length ) {
                throw new Exception( "Too many arguments." );
            }

            // create a new scope with the args for the method
            var scope = runtime.CreateScope();
            for ( int idx = 0; idx < args.Length; ++idx ) {
                scope.SetVariable( method.Parameters[idx].Name, args[idx] );
            }

            return method.Execute( scope );
        }

        private object Evaluate( MemberAccessExpressionSyntax m, Runtime runtime ) {
            if ( runtime.TryGetVariableValue( (string)m.MemberName.Value, out var value ) ) {
                return value;
            }

            throw new Exception( $"Unknown variable '{m.MemberName.Value}'" );
        }

    }
}
