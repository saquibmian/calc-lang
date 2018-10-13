using System;

namespace CalcLang.CodeAnalysis.Binding {
    public sealed class BoundUnaryExpresion : BoundExpression {
        internal BoundUnaryExpresion( BoundUnaryOperatorKind operatorKind, BoundExpression expression ) {
            OperatorKind = operatorKind;
            Expression = expression;
        }

        public BoundUnaryOperatorKind OperatorKind { get; }
        public BoundExpression Expression { get; }

        public override Type ReturnType => Expression.ReturnType;

        public override BoundNodeKind Kind => BoundNodeKind.UnaryExpression;
    }

}