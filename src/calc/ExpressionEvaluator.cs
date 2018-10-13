using System;
using System.Collections.Generic;
using System.Linq;
using CalcLang.CodeAnalysis;
using CalcLang.CodeAnalysis.Binding;
using CalcLang.CodeAnalysis.Syntax;

namespace CalcLang {
    public sealed class ExpressionEvaluator {

        public object Evaluate( BoundExpression expression ) {
            switch ( expression ) {
                case BoundLiteralExpression n:
                    return n.Value;

                case BoundUnaryExpresion u:
                    return Evaluate( u );

                case BoundBinaryExpression b:
                    return Evaluate( b );

                // TODO
                // case InvocationExpressionSyntax i:
                //     return Evaluate( i );

                // case MemberAccessExpressionSyntax m:
                //     return Evaluate( m );

                default:
                    throw new Exception( $"Unexpected expression {expression.Kind}" );
            }
        }

        private object Evaluate( BoundUnaryExpresion u ) {
            var expr = Evaluate( u.Expression );

            switch ( u.OperatorKind ) {
                case BoundUnaryOperatorKind.Identity:
                    return (int)expr;
                case BoundUnaryOperatorKind.Negation:
                    return -(int)expr;
                case BoundUnaryOperatorKind.LogicalNot:
                    return !(bool)expr;

                default:
                    throw new Exception( $"Unexpected unary operator {u.OperatorKind}" );
            }
        }

        private object Evaluate( BoundBinaryExpression b ) {
            var left = Evaluate( b.Left );
            var right = Evaluate( b.Right );

            switch ( b.OperatorKind ) {
                case BoundBinaryOperatorKind.Equality:
                    return left.Equals( right );
                case BoundBinaryOperatorKind.Addition:
                    return (int)left + (int)right;
                case BoundBinaryOperatorKind.Subtraction:
                    return (int)left - (int)right;
                case BoundBinaryOperatorKind.Multiplication:
                    return (int)left * (int)right;
                case BoundBinaryOperatorKind.Division:
                    return (int)left / (int)right;
                case BoundBinaryOperatorKind.LogicalAnd:
                    return (bool)left && (bool)right;
                case BoundBinaryOperatorKind.LogicalOr:
                    return (bool)left || (bool)right;

                default:
                    throw new Exception( $"Unexpected binary operator {b.OperatorKind}" );
            }
        }

        // TODO
        // private object Evaluate( InvocationExpressionSyntax i ) {
        //     // TODO: find a way to determine the return type of the expression
        //     var args = i.ArgumentList.Arguments.Nodes
        //         .Select( arg => Evaluate( arg.Expression ) )
        //         .ToArray();
        //     var argTypes = args.Select( a => a.GetType() ).ToArray();

        //     var method .GetMethod( (string)i.Member.MemberName.Value, argTypes );
        //     if ( method == null ) {
        //         throw new Exception( $"Unknown function '{i.Member.MemberName.Value}'" );
        //     }

        //     if ( i.ArgumentList.Arguments.Nodes.Count() > method.Parameters.Length ) {
        //         throw new Exception( "Too many arguments." );
        //     }

        //     // create a new scope with the args for the method
        //     var scope .CreateScope();
        //     for ( int idx = 0; idx < args.Length; ++idx ) {
        //         scope.SetVariable( method.Parameters[idx].Name, args[idx] );
        //     }

        //     return method.Execute( scope );
        // }

        // private object Evaluate( MemberAccessExpressionSyntax m ) {
        //     if .TryGetVariableValue( (string)m.MemberName.Value, out var value ) ) {
        //         return value;
        //     }

        //     throw new Exception( $"Unknown variable '{m.MemberName.Value}'" );
        // }

    }
}
