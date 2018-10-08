namespace CalcLang.CodeAnalysis {
    enum SyntaxKind {
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

        NumberExpression,
        Statement,
        BinaryExpression,
        ParenthetizedExpression,
        InvocationExpression,
        ArgumentList,
        SeparatedSyntaxList,
        Argument,
        MemberAccessExpression
    }
}
