namespace CalcLang.CodeAnalysis.Syntax {
    public enum SyntaxKind {

        // Tokens
        BadToken,
        EndOfFileToken,
        WhiteSpaceToken,
        PlusToken,
        MinusToken,
        ForwardSlashToken,
        StarToken,
        OpenParenthesisToken,
        CloseParenthesisToken,
        CommaToken,
        DotToken,
        EqualsToken,
        EqualsEqualsToken,
        BangEqualsToken,
        PipeToken,
        PipePipeToken,
        AmpersandToken,
        AmpersandAmpersandToken,
        BangToken,
        IntegerToken,
        FloatToken,
        IdentiferToken,

        // Keywords
        TrueKeyword,
        FalseKeyword,

        // Expressions
        IntegerLiteralExpression,
        FloatLiteralExpression,
        BooleanLiteralExpression,
        UnaryExpression,
        BinaryExpression,
        ParenthetizedExpression,
        MemberAccessExpression,
        InvocationExpression,
        SeparatedSyntaxList,
        ArgumentList,
        Argument,
    }
}
