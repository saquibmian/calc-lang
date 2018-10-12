namespace CalcLang.CodeAnalysis {
    public static class SyntaxFacts {

        public static int GetUnaryOperatorPrecendence( this SyntaxToken token ) {
            switch ( token.Kind ) {
                case SyntaxKind.PlusToken:
                case SyntaxKind.MinusToken:
                    return 3;

                default:
                    return 0;
            }
        }

        public static int GetBinaryOperatorPrecendence( this SyntaxToken token ) {
            switch ( token.Kind ) {
                case SyntaxKind.ForwardSlashToken:
                case SyntaxKind.StarToken:
                    return 2;

                case SyntaxKind.PlusToken:
                case SyntaxKind.MinusToken:
                    return 1;

                default:
                    return 0;
            }
        }

    }
}
