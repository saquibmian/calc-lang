using System;
using System.Collections.Immutable;
using System.Linq;
using CalcLang.CodeAnalysis.Syntax;

namespace CalcLang.CodeAnalysis.Binding {
    public sealed class BoundBinaryOperator {

        private static readonly ImmutableArray<BoundBinaryOperator> _ops = ImmutableArray.Create(
            new BoundBinaryOperator( SyntaxKind.PlusToken, BoundBinaryOperatorKind.Addition, typeof( int ) ),
            new BoundBinaryOperator( SyntaxKind.MinusToken, BoundBinaryOperatorKind.Subtraction, typeof( int ) ),
            new BoundBinaryOperator( SyntaxKind.ForwardSlashToken, BoundBinaryOperatorKind.Division, typeof( int ) ),
            new BoundBinaryOperator( SyntaxKind.StarToken, BoundBinaryOperatorKind.Multiplication, typeof( int ) ),

            new BoundBinaryOperator( SyntaxKind.AmpersandAmpersandToken, BoundBinaryOperatorKind.LogicalAnd, typeof( bool ) ),
            new BoundBinaryOperator( SyntaxKind.PipePipeToken, BoundBinaryOperatorKind.LogicalOr, typeof( bool ) ),

            new BoundBinaryOperator( SyntaxKind.EqualsEqualsToken, BoundBinaryOperatorKind.Equality, typeof( int ), typeof( int ), typeof( bool ) ),
            new BoundBinaryOperator( SyntaxKind.EqualsEqualsToken, BoundBinaryOperatorKind.Equality, typeof( bool ), typeof( bool ), typeof( bool ) )
        );

        private BoundBinaryOperator( SyntaxKind syntaxKind, BoundBinaryOperatorKind kind, Type operandType )
        : this( syntaxKind, kind, operandType, operandType, operandType ) { }

        private BoundBinaryOperator( SyntaxKind syntaxKind, BoundBinaryOperatorKind kind, Type leftType, Type rightType, Type returnType ) {
            SyntaxKind = syntaxKind;
            Kind = kind;
            LeftType = leftType;
            RightType = rightType;
            ReturnType = returnType;
        }

        public SyntaxKind SyntaxKind { get; }
        public BoundBinaryOperatorKind Kind { get; }
        public Type LeftType { get; }
        public Type RightType { get; }
        public Type ReturnType { get; }

        public static BoundBinaryOperator Bind( SyntaxKind syntaxKind, Type leftType, Type rightType ) {
            return _ops.FirstOrDefault( op => op.SyntaxKind == syntaxKind && op.LeftType == leftType && op.RightType == rightType );
        }
    }

}