using System;
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
    }
}
