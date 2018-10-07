using System;
using System.Threading.Tasks;
using CalcLang.CodeAnalysis;

namespace CalcLang {
    internal sealed class Program {
        internal static void Main( string[] args ) {
            var interpreter = new Interpreter();
            interpreter.Run();
        }
    }
}
