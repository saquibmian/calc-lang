using System.Collections.Generic;
using System.Collections.Immutable;

namespace CalcLang.CodeAnalysis.Syntax {
    internal static class LexerExtensions {
        internal static ImmutableArray<SyntaxToken> ReadAllTokens( this Lexer lexer ) {
            var tokens = new List<SyntaxToken>();

            SyntaxToken token;
            do {
                token = lexer.Lex();

                if ( token.Kind != SyntaxKind.WhiteSpaceToken && token.Kind != SyntaxKind.BadToken ) {
                    tokens.Add( token );
                }
            } while ( token.Kind != SyntaxKind.EndOfFileToken );

            return tokens.ToImmutableArray();
        }
    }
}
