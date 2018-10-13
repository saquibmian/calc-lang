using System;
using System.Collections.Generic;
using System.Linq;
using CalcLang.CodeAnalysis;
using CalcLang.CodeAnalysis.Binding;
using CalcLang.CodeAnalysis.Syntax;

namespace CalcLang {
    public sealed class ExpressionEvaluator {

        public object Evaluate( BoundExpression expression, Runtime runtime ) {
            switch ( expression ) {
                case BoundLiteralExpression n:
                    return n.Value;

                case BoundUnaryExpresion u:
                    return Evaluate( u, runtime );

                case BoundBinaryExpression b:
                    return Evaluate( b, runtime );

                // TODO
                // case InvocationExpressionSyntax i:
                //     return Evaluate( i, runtime );

                // case MemberAccessExpressionSyntax m:
                //     return Evaluate( m, runtime );

                default:
                    throw new Exception( $"Unexpected expression {expression.Kind}" );
            }
        }

        private object Evaluate( BoundUnaryExpresion u, Runtime runtime ) {
            var expr = (int)Evaluate( u.Expression, runtime );

            switch ( u.OperatorKind ) {
                case BoundUnaryOperatorKind.Identity:
                    return expr;
                case BoundUnaryOperatorKind.Negation:
                    return -expr;

                default:
                    throw new Exception( $"The operator {u.OperatorKind} is not defined for type {u.Expression.ReturnType}" );
            }
        }

        private object Evaluate( BoundBinaryExpression b, Runtime runtime ) {
            var left = (int)Evaluate( b.Left, runtime );
            var right = (int)Evaluate( b.Right, runtime );

            switch ( b.OperatorKind ) {
                case BoundBinaryOperatorKind.Addition:
                    return left + right;
                case BoundBinaryOperatorKind.Subtraction:
                    return left - right;
                case BoundBinaryOperatorKind.Multiplication:
                    return left * right;
                case BoundBinaryOperatorKind.Division:
                    return left / right;

                default:
                    throw new Exception( $"The operator {b.OperatorKind} is not defined for types {b.Left.ReturnType} and {b.Right.ReturnType}" );
            }
        }

        // TODO
        // private object Evaluate( InvocationExpressionSyntax i, Runtime runtime ) {
        //     // TODO: find a way to determine the return type of the expression
        //     var args = i.ArgumentList.Arguments.Nodes
        //         .Select( arg => Evaluate( arg.Expression, runtime ) )
        //         .ToArray();
        //     var argTypes = args.Select( a => a.GetType() ).ToArray();

        //     var method = runtime.GetMethod( (string)i.Member.MemberName.Value, argTypes );
        //     if ( method == null ) {
        //         throw new Exception( $"Unknown function '{i.Member.MemberName.Value}'" );
        //     }

        //     if ( i.ArgumentList.Arguments.Nodes.Count() > method.Parameters.Length ) {
        //         throw new Exception( "Too many arguments." );
        //     }

        //     // create a new scope with the args for the method
        //     var scope = runtime.CreateScope();
        //     for ( int idx = 0; idx < args.Length; ++idx ) {
        //         scope.SetVariable( method.Parameters[idx].Name, args[idx] );
        //     }

        //     return method.Execute( scope );
        // }

        // private object Evaluate( MemberAccessExpressionSyntax m, Runtime runtime ) {
        //     if ( runtime.TryGetVariableValue( (string)m.MemberName.Value, out var value ) ) {
        //         return value;
        //     }

        //     throw new Exception( $"Unknown variable '{m.MemberName.Value}'" );
        // }

    }
}
