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
        NumberExpression,
        Statement,
        BinaryExpression,
        ParenthetizedExpression
    }
}
