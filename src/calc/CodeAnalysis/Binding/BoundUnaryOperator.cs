using System;
using System.Collections.Immutable;
using System.Linq;
using CalcLang.CodeAnalysis.Syntax;

namespace CalcLang.CodeAnalysis.Binding {
    public sealed class BoundUnaryOperator {

        private readonly static ImmutableArray<BoundUnaryOperator> _ops = ImmutableArray.Create(
            new BoundUnaryOperator( SyntaxKind.BangToken, BoundUnaryOperatorKind.LogicalNot, typeof( bool ) ),
            new BoundUnaryOperator( SyntaxKind.PlusToken, BoundUnaryOperatorKind.Identity, typeof( int ) ),
            new BoundUnaryOperator( SyntaxKind.MinusToken, BoundUnaryOperatorKind.Negation, typeof( int ) )
        );

        private BoundUnaryOperator( SyntaxKind syntaxKind, BoundUnaryOperatorKind kind, Type operandType )
        : this( syntaxKind, kind, operandType, operandType ) { }

        private BoundUnaryOperator( SyntaxKind syntaxKind, BoundUnaryOperatorKind kind, Type operandType, Type returnType ) {
            SyntaxKind = syntaxKind;
            Kind = kind;
            OperandType = operandType;
            ReturnType = returnType;
        }

        public SyntaxKind SyntaxKind { get; }
        public BoundUnaryOperatorKind Kind { get; }
        public Type OperandType { get; }
        public Type ReturnType { get; }

        public static BoundUnaryOperator Bind( SyntaxKind syntaxKind, Type operandType ) {
            return _ops.FirstOrDefault( op => op.SyntaxKind == syntaxKind && op.OperandType == operandType );
        }
    }

}