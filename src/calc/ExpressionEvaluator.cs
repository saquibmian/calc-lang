using System;
using System.Linq;
using CalcLang.CodeAnalysis;

namespace CalcLang {
    internal sealed class ExpressionEvaluator {
        internal int Evaluate( ExpressionSyntax expression ) {
            switch ( expression ) {
                case NumberExpressionSyntax n:
                    return (int)n.NumberToken.Value;

                case BinaryExpressionSyntax b:
                    return Evaluate( b );

                case ParenthetizedExpressionSyntax p:
                    return Evaluate( p.Expression );

                case InvocationExpressionSyntax i:
                    return Evaluate( i );

                default:
                    throw new Exception( $"Unexpected expression {expression.Kind}" );
            }
        }

        private int Evaluate( BinaryExpressionSyntax b ) {
            var left = Evaluate( b.Left );
            var right = Evaluate( b.Right );

            switch ( b.OperatorToken.Kind ) {
                case SyntaxKind.PlusToken:
                    return left + right;
                case SyntaxKind.MinusToken:
                    return left - right;
                case SyntaxKind.StarToken:
                    return left * right;
                case SyntaxKind.ForwardSlashToken:
                    return left / right;

                default:
                    throw new Exception( $"Unexpected binary operator {b.OperatorToken.Kind}" );
            }
        }

        private int Evaluate( InvocationExpressionSyntax i ) {
            switch ( i.Identifer.Value ) {
                case "sum":
                    return i.ArgumentList.Arguments.Nodes.Select( arg => Evaluate( arg.Expression ) ).Sum();

                case "min":
                    return i.ArgumentList.Arguments.Nodes.Select( arg => Evaluate( arg.Expression ) ).Min();

                case "max":
                    return i.ArgumentList.Arguments.Nodes.Select( arg => Evaluate( arg.Expression ) ).Max();

                default:
                    throw new Exception( $"Unknown function {i.Identifer.Value}" );
            }
        }
    }
}
