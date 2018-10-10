namespace CalcLang.CodeAnalysis {
    public abstract class ConstantExpressionSyntax : ExpressionSyntax {
        public abstract object Value { get; }
    }
}
