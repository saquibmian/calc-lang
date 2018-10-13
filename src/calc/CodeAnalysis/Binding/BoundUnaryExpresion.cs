using System;

namespace CalcLang.CodeAnalysis.Binding {
    public sealed class BoundUnaryExpresion : BoundExpression {
        internal BoundUnaryExpresion( BoundUnaryOperator op, BoundExpression expression ) {
            Op = op;
            Expression = expression;
        }

        public BoundUnaryOperator Op { get; }
        public BoundExpression Expression { get; }
        public override Type ReturnType => Op.ReturnType;
        public override BoundNodeKind Kind => BoundNodeKind.UnaryExpression;
    }

}