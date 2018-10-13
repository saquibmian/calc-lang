using System;
using System.Linq;

namespace CalcLang.CodeAnalysis.Binding {
    public sealed class BoundBinaryExpression : BoundExpression {
        internal BoundBinaryExpression( BoundExpression left, BoundBinaryOperator op, BoundExpression right ) {
            Left = left;
            Op = op;
            Right = right;
        }

        public BoundExpression Left { get; }
        public BoundBinaryOperator Op { get; }
        public BoundExpression Right { get; }
        public override Type ReturnType => Op.ReturnType;
        public override BoundNodeKind Kind => BoundNodeKind.BinaryExpression;
    }
}