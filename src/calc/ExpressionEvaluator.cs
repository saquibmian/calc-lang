using System;
using System.Collections.Generic;
using System.Linq;
using CalcLang.CodeAnalysis;

namespace CalcLang {
    internal sealed class ExpressionEvaluator {
        private readonly IDictionary<string, int> _variablesByName = new Dictionary<string, int> {
            ["PI"] = (int)Math.PI
        };

        internal int Evaluate( StatementSyntax statement ) {
            switch ( statement ) {
                case ExpressionStatementSyntax e:
                    return Evaluate( e.Expression );

                default:
                    throw new Exception( $"Unexpected expression {statement.Kind}" );
            }
        }

        private int Evaluate( ExpressionSyntax expression ) {
            switch ( expression ) {
                case NumberExpressionSyntax n:
                    return (int)n.NumberToken.Value;

                case BinaryExpressionSyntax b:
                    return Evaluate( b );

                case ParenthetizedExpressionSyntax p:
                    return Evaluate( p.Expression );

                case InvocationExpressionSyntax i:
                    return Evaluate( i );

                case MemberAccessExpressionSyntax m:
                    return Evaluate( m );

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
                    throw new Exception( $"Unknown function '{i.Identifer.Value}'" );
            }
        }

        private int Evaluate( MemberAccessExpressionSyntax m ) {
            if ( _variablesByName.TryGetValue( (string)m.MemberName.Value, out var value ) ) {
                return value;
            }

            throw new Exception( $"Unknown variable '{m.MemberName.Value}'" );
        }
    }
}
