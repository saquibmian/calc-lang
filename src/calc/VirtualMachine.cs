using System;
using CalcLang.CodeAnalysis;

namespace CalcLang {
    internal sealed class VirtualMachine {

        private readonly ExpressionEvaluator _evaluator = new ExpressionEvaluator();

        internal object Run( StatementSyntax statement, Runtime runtime ) {
            switch ( statement ) {
                case ExpressionStatementSyntax e:
                    return _evaluator.Evaluate( e.Expression, runtime );

                case LocalDeclarationStatementSyntax l:
                    return Evaluate( l, runtime );

                default:
                    throw new Exception( $"Unexpected expression {statement.Kind}" );
            }
        }

        private object Evaluate( LocalDeclarationStatementSyntax local, Runtime runtime ) {
            var value = _evaluator.Evaluate( local.Expression, runtime );
            runtime.SetVariable( (string)local.NameToken.Value, value );
            return null;
        }

    }
}
