using System;
using System.IO;
using System.Threading.Tasks;

namespace CalcLang {
    internal sealed class StatementReader {
        private readonly TextReader _reader;

        public StatementReader( TextReader reader ) {
            _reader = reader ?? throw new ArgumentNullException( nameof( reader ) );
        }

        internal Task<string> ReadStatementAsync() {
            
            return _reader.ReadLineAsync();
        }
    }
}
