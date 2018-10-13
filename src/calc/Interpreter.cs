using System;
using System.IO;
using System.Threading.Tasks;
using CalcLang.CodeAnalysis;
using CalcLang.CodeAnalysis.Binding;
using CalcLang.CodeAnalysis.Syntax;

namespace CalcLang {
    internal sealed class Interpreter {
        private readonly Binder _binder = new Binder();
        private readonly VirtualMachine _vm = new VirtualMachine();
        private readonly Runtime _runtime = Runtime.Global.CreateScope();

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

            if ( !tree.Diagnostics.IsEmpty ) {
                using ( new OutputColor( foreground: ConsoleColor.Red ) ) {
                    foreach ( var diag in tree.Diagnostics ) {
                        Console.WriteLine( $"Error at position {diag.Position}: {diag.Message}" );
                    }
                    return;
                }
            }

            try {
                var bound = _binder.BindExpression( (ExpressionSyntax)tree.Root );
                var result = _vm.Run( bound, _runtime );
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
