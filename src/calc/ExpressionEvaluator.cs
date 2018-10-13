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

            switch ( u.Op.Kind ) {
                case BoundUnaryOperatorKind.Identity:
                    return (int)expr;
                case BoundUnaryOperatorKind.Negation:
                    return -(int)expr;
                case BoundUnaryOperatorKind.LogicalNot:
                    return !(bool)expr;

                default:
                    throw new Exception( $"Unexpected unary operator {u.Op}" );
            }
        }

        private object Evaluate( BoundBinaryExpression b ) {
            var left = Evaluate( b.Left );
            var right = Evaluate( b.Right );

            switch ( b.Op.Kind ) {
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
                    throw new Exception( $"Unexpected binary operator {b.Op}" );
            }
        }
    }
}
