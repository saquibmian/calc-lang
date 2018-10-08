using System;

namespace CalcLang {
    internal sealed class OutputColor : IDisposable {
        private readonly ConsoleColor? _prevForeground;

        public OutputColor( ConsoleColor? foreground = null ) {
            if ( foreground.HasValue ) {
                _prevForeground = Console.ForegroundColor;
                Console.ForegroundColor = foreground.Value;
            }
        }

        public void Dispose() {
            if ( _prevForeground.HasValue ) {
                Console.ForegroundColor = _prevForeground.Value;
            }
        }
    }
}
