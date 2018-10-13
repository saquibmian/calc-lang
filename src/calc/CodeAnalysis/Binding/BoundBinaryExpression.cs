using System;

namespace CalcLang.CodeAnalysis.Binding {
    public sealed class BoundBinaryExpression : BoundExpression {
        internal BoundBinaryExpression( BoundExpression left, BoundBinaryOperatorKind operatorKind, BoundExpression right ) {
            Left = left;
            OperatorKind = operatorKind;
            Right = right;
        }

        public BoundExpression Left { get; }
        public BoundBinaryOperatorKind OperatorKind { get; }
        public BoundExpression Right { get; }

        public override Type ReturnType => Left.ReturnType;

        public override BoundNodeKind Kind => BoundNodeKind.BinaryExpression;
    }

}