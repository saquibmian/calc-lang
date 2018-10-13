using System;
using System.Collections.Generic;
using CalcLang.CodeAnalysis;
using CalcLang.CodeAnalysis.Syntax;

namespace CalcLang.CodeAnalysis.Binding {
    internal sealed class Binder {

        private readonly List<Diagnostic> _diagnostics = new List<Diagnostic>();

        public IEnumerable<Diagnostic> Diagnostics => _diagnostics;

        internal BoundExpression BindExpression( ExpressionSyntax syntax ) {
            switch ( syntax.Kind ) {
                case SyntaxKind.IntegerLiteralExpression:
                case SyntaxKind.FloatLiteralExpression:
                case SyntaxKind.BooleanLiteralExpression:
                    return BindConstantExpression( (ConstantExpressionSyntax)syntax );

                case SyntaxKind.UnaryExpression:
                    return BindUnaryExpression( (UnaryExpressionSyntax)syntax );

                case SyntaxKind.BinaryExpression:
                    return BindBinaryExpression( (BinaryExpressionSyntax)syntax );

                case SyntaxKind.ParenthetizedExpression:
                    return BinaryParenthetizedExpression( (ParenthetizedExpressionSyntax)syntax );

                default:
                    throw new Exception( $"Unexpected syntax kind {syntax.Kind}" );
            }
        }

        private BoundExpression BinaryParenthetizedExpression( ParenthetizedExpressionSyntax syntax ) {
            return BindExpression( syntax.Expression );
        }

        private BoundExpression BindBinaryExpression( BinaryExpressionSyntax syntax ) {
            var left = BindExpression( syntax.Left );
            var right = BindExpression( syntax.Right );

            var op = BoundBinaryOperator.Bind( syntax.OperatorToken.Kind, left.ReturnType, right.ReturnType );
            if ( op == null ) {
                _diagnostics.Add( new Diagnostic( 0, $"Binary operator '{syntax.OperatorToken.ValueText}' is not defined for types {left.ReturnType} and {right.ReturnType}." ) );
                return left;
            }

            return new BoundBinaryExpression( left, op, right );
        }

        private BoundExpression BindUnaryExpression( UnaryExpressionSyntax syntax ) {
            var boundExpresion = BindExpression( syntax.Expression );

            var op = BoundUnaryOperator.Bind( syntax.OperatorToken.Kind, boundExpresion.ReturnType );
            if ( op == null ) {
                _diagnostics.Add( new Diagnostic( 0, $"Unary operator '{syntax.OperatorToken.ValueText}' is not defined for type {boundExpresion.ReturnType}." ) );
                return boundExpresion;
            }

            return new BoundUnaryExpresion( op, boundExpresion );
        }

        internal BoundLiteralExpression BindConstantExpression( ConstantExpressionSyntax syntax ) {
            return new BoundLiteralExpression( syntax.Value );
        }
    }
}