using System;
using System.Collections.Generic;
using System.Linq;
using CalcLang.CodeAnalysis;

namespace CalcLang {
    internal sealed class ExpressionEvaluator {
        
        internal int? Evaluate( StatementSyntax statement, Runtime runtime ) {
            switch ( statement ) {
                case ExpressionStatementSyntax e:
                    return Evaluate( e.Expression, runtime );

                case LocalDeclarationStatementSyntax l:
                    return Evaluate( l, runtime );

                default:
                    throw new Exception( $"Unexpected expression {statement.Kind}" );
            }
        }

        private int? Evaluate( ExpressionSyntax expression, Runtime runtime ) {
            switch ( expression ) {
                case NumberExpressionSyntax n:
                    return (int)n.NumberToken.Value;

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

        private int? Evaluate( BinaryExpressionSyntax b, Runtime runtime ) {
            var left = Evaluate( b.Left, runtime );
            var right = Evaluate( b.Right, runtime );

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

        private int? Evaluate( InvocationExpressionSyntax i, Runtime runtime ) {
            switch ( i.Identifer.Value ) {
                case "sum":
                    return i.ArgumentList.Arguments.Nodes.Select( arg => Evaluate( arg.Expression, runtime ) ).Sum();

                case "min":
                    return i.ArgumentList.Arguments.Nodes.Select( arg => Evaluate( arg.Expression, runtime ) ).Min();

                case "max":
                    return i.ArgumentList.Arguments.Nodes.Select( arg => Evaluate( arg.Expression, runtime ) ).Max();

                default:
                    throw new Exception( $"Unknown function '{i.Identifer.Value}'" );
            }
        }

        private int? Evaluate( MemberAccessExpressionSyntax m, Runtime runtime ) {
            if ( runtime.TryGetVariableValue( (string)m.MemberName.Value, out var value ) ) {
                return value;
            }

            throw new Exception( $"Unknown variable '{m.MemberName.Value}'" );
        }

        private int? Evaluate( LocalDeclarationStatementSyntax local, Runtime runtime ) {
            var value = Evaluate( local.Expression, runtime );
            runtime.SetVariable( (string)local.NameToken.Value, value.Value );
            return null;
        }
    }
}
