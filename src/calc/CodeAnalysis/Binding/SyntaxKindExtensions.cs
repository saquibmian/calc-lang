using System;
using CalcLang.CodeAnalysis.Syntax;

namespace CalcLang.CodeAnalysis.Binding {
    internal static class SyntaxKindExtensions {
        internal static BoundUnaryOperatorKind ToUnaryOperatorKind( this SyntaxKind kind ) {
            switch ( kind ) {
                case SyntaxKind.PlusToken:
                    return BoundUnaryOperatorKind.Identity;
                case SyntaxKind.MinusToken:
                    return BoundUnaryOperatorKind.Negation;

                default:
                    throw new Exception( $"Unexpected unary operator kind: {kind}" );
            }
        }
        internal static BoundBinaryOperatorKind ToBinaryOperatorKind( this SyntaxKind kind ) {
            switch ( kind ) {
                case SyntaxKind.PlusToken:
                    return BoundBinaryOperatorKind.Addition;
                case SyntaxKind.MinusToken:
                    return BoundBinaryOperatorKind.Subtraction;
                case SyntaxKind.ForwardSlashToken:
                    return BoundBinaryOperatorKind.Division;
                case SyntaxKind.StarToken:
                    return BoundBinaryOperatorKind.Multiplication;

                default:
                    throw new Exception( $"Unexpected binary operator kind: {kind}" );
            }
        }
    }

}