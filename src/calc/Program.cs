using System;
using System.Threading.Tasks;
using CalcLang.CodeAnalysis;

namespace CalcLang {
    internal sealed class Program {
        internal static async Task Main( string[] args ) {
            var reader = new StatementReader( Console.In );
            var interpreter = new Interpreter( Console.Out );

            while ( !interpreter.Done ) {
                await Console.Out.WriteAsync( "> " );
                var statement = await reader.ReadStatementAsync();
                await interpreter.ExecuteAsync( statement );
            }
        }
    }
}
