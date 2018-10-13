using System;
using CalcLang.CodeAnalysis;
using CalcLang.CodeAnalysis.Binding;

namespace CalcLang {
    internal sealed class VirtualMachine {

        private readonly ExpressionEvaluator _evaluator = new ExpressionEvaluator();

        internal object Run( BoundExpression statement, Runtime runtime ) {
            return _evaluator.Evaluate( statement, runtime );
        }

        // TODO
        // private object Evaluate( LocalDeclarationStatementSyntax local, Runtime runtime ) {
        //     var value = _evaluator.Evaluate( local.Expression, runtime );
        //     runtime.SetVariable( (string)local.NameToken.Value, value );
        //     return null;
        // }

    }
}
