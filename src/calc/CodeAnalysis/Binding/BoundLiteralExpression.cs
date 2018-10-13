using System;

namespace CalcLang.CodeAnalysis.Binding {
    public sealed class BoundLiteralExpression : BoundExpression {
        internal BoundLiteralExpression( object value ) {
            Value = value;
        }

        public object Value { get; }

        public override Type ReturnType => Value.GetType();

        public override BoundNodeKind Kind => BoundNodeKind.LiteralExpression;
    }

}