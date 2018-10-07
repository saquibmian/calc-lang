using System;
using System.IO;
using System.Threading.Tasks;
using CalcLang.CodeAnalysis;

namespace CalcLang {
    internal sealed class Interpreter {

        private readonly ExpressionEvaluator _evaluator = new ExpressionEvaluator();
        private readonly TextWriter _out;

        private bool _printTree;

        internal Interpreter( TextWriter output ) {
            _out = output ?? throw new ArgumentNullException( nameof( output ) );
        }

        internal bool Done { get; private set; }

        internal Task ExecuteAsync( string statement ) {
            if ( TryGetHandlerForCommand( statement, out var commandHandler ) ) {
                return commandHandler;
            }

            return HandleStatementAsync( statement );
        }

        internal bool TryGetHandlerForCommand( string statement, out Task handler ) {
            switch ( statement ) {
                case "#showTree":
                    handler = HandleShowTree();
                    return true;

                case "quit":
                    handler = HandleQuitAsync();
                    return true;

                default:
                    handler = default;
                    return false;
            }
        }

        internal async Task HandleStatementAsync( string statement ) {
            var tree = SyntaxParser.Parse( statement );

            if ( _printTree ) {
                await _out.WriteLineAsync( SyntaxTreePrinter.PrintTree( tree ) );
            }

            if ( !tree.Diagnostics.IsEmpty ) {
                foreach ( var diag in tree.Diagnostics ) {
                    await _out.WriteLineAsync( $"Error at position {diag.Position}: {diag.Message}" );
                }
                await HandleInvalidStatement( statement );
                return;
            }

            var result = _evaluator.Evaluate( tree.Root.Expression );
            await _out.WriteLineAsync( $"{result}" );
        }

        private Task HandleShowTree() {
            _printTree = !_printTree;
            return Task.CompletedTask;
        }

        private Task HandleQuitAsync() {
            Done = true;
            return _out.WriteLineAsync( "done" );
        }

        private Task HandleInvalidStatement( string statement ) {
            return _out.WriteLineAsync( $"ERROR: Invalid statement '{statement}'" );
        }

    }
}
