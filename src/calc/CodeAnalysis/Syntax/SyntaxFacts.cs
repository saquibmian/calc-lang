namespace CalcLang.CodeAnalysis.Syntax {
    public static class SyntaxFacts {

        public static int GetUnaryOperatorPrecendence( this SyntaxToken token ) {
            switch ( token.Kind ) {
                case SyntaxKind.PlusToken:
                case SyntaxKind.MinusToken:
                case SyntaxKind.BangToken:
                    return 6;

                default:
                    return 0;
            }
        }

        public static int GetBinaryOperatorPrecendence( this SyntaxToken token ) {
            switch ( token.Kind ) {
                case SyntaxKind.ForwardSlashToken:
                case SyntaxKind.StarToken:
                    return 5;

                case SyntaxKind.PlusToken:
                case SyntaxKind.MinusToken:
                    return 4;

                case SyntaxKind.EqualsEqualsToken:
                case SyntaxKind.BangEqualsToken:
                    return 3;

                case SyntaxKind.AmpersandToken:
                case SyntaxKind.AmpersandAmpersandToken:
                    return 2;

                case SyntaxKind.PipeToken:
                case SyntaxKind.PipePipeToken:
                    return 1;

                default:
                    return 0;
            }
        }

        public static SyntaxKind GetKeywordKind( this string keyword ) {
            switch ( keyword ) {
                case "true":
                    return SyntaxKind.TrueKeyword;
                case "false":
                    return SyntaxKind.FalseKeyword;

                default:
                    return SyntaxKind.IdentiferToken;
            }
        }

    }
}
