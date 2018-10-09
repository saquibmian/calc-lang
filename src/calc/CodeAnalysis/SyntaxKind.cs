namespace CalcLang.CodeAnalysis {
    public enum SyntaxKind {
        IntegerToken,
        EndOfFileToken,
        WhiteSpaceToken,
        IdentiferToken,
        PlusToken,
        MinusToken,
        ForwardSlashToken,
        StarToken,
        OpenParenthesisToken,
        CloseParenthesisToken,
        BadToken,
        CommaToken,

        IntegerLiteralExpression,
        ExpressionStatement,
        BinaryExpression,
        ParenthetizedExpression,
        InvocationExpression,
        ArgumentList,
        SeparatedSyntaxList,
        Argument,
        MemberAccessExpression,
        EqualsToken,
        LocalDeclarationStatement,
        FloatLiteralExpression,
        FloatToken
    }
}
