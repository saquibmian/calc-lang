using System;
using CalcLang.CodeAnalysis.Syntax;

namespace CalcLang.CodeAnalysis.Binding {
    internal sealed class Binder {
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
            var operatorKind = syntax.OperatorToken.Kind.ToBinaryOperatorKind();
            return new BoundBinaryExpression( left, operatorKind, right );
        }

        private BoundExpression BindUnaryExpression( UnaryExpressionSyntax syntax ) {
            var boundExpresion = BindExpression( syntax.Expression );
            var operatorKind = syntax.OperatorToken.Kind.ToUnaryOperatorKind();
            return new BoundUnaryExpresion( operatorKind, boundExpresion );
        }

        internal BoundLiteralExpression BindConstantExpression( ConstantExpressionSyntax syntax ) {
            return new BoundLiteralExpression( syntax.Value );
        }
    }
}