namespace CalcLang.CodeAnalysis.Syntax {
    public abstract class ConstantExpressionSyntax : ExpressionSyntax {
        public abstract object Value { get; }
    }
}
