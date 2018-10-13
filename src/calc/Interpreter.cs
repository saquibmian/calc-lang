using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CalcLang.CodeAnalysis;
using CalcLang.CodeAnalysis.Binding;
using CalcLang.CodeAnalysis.Syntax;

namespace CalcLang {
    internal sealed class Interpreter {
        private readonly ExpressionEvaluator _evaluator = new ExpressionEvaluator();

        private bool _printTree;
        private bool _done;

        internal void Run() {
            while ( !_done ) {
                Console.Write( "> " );
                var statement = Console.ReadLine();
                Execute( statement );
            }
        }

        private void Execute( string statement ) {
            if ( TryHandleCommand( statement ) ) {
                return;
            }

            HandleStatementAsync( statement );
        }

        private bool TryHandleCommand( string statement ) {
            switch ( statement ) {
                case "#showTree":
                    HandleShowTree();
                    return true;

                case "quit":
                    HandleQuitAsync();
                    return true;

                default:
                    return false;
            }
        }

        private void HandleStatementAsync( string statement ) {
            var tree = SyntaxTree.Parse( statement );

            if ( _printTree ) {
                using ( new OutputColor( foreground: ConsoleColor.DarkGreen ) ) {
                    Console.WriteLine( SyntaxTreePrinter.PrintTree( tree ) );
                }
            }

            var binder = new Binder();
            var bound = binder.BindExpression( (ExpressionSyntax)tree.Root );

            var diagnostics = Enumerable.Concat( tree.Diagnostics, binder.Diagnostics ).ToArray();
            if ( diagnostics.Length > 0 ) {
                using ( new OutputColor( foreground: ConsoleColor.Red ) ) {
                    foreach ( var diag in diagnostics ) {
                        Console.WriteLine( $"Error at position {diag.Position}: {diag.Message}" );
                    }
                    return;
                }
            }

            try {
                var result = _evaluator.Evaluate( bound );
                Console.WriteLine( result );
            } catch ( Exception e ) {
                using ( new OutputColor( foreground: ConsoleColor.Red ) ) {
                    Console.WriteLine( e.Message );
                }
            }
        }

        private void HandleShowTree() {
            _printTree = !_printTree;
        }

        private void HandleQuitAsync() {
            _done = true;
        }

    }
}
