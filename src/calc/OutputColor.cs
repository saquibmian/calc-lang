using System;

namespace CalcLang {
    internal sealed class OutputColor : IDisposable {
        private readonly ConsoleColor? _foreground;
        private readonly ConsoleColor? _prevForeground;

        public OutputColor( ConsoleColor? foreground = null ) {
            this._foreground = foreground;

            if ( _foreground.HasValue ) {
                _prevForeground = Console.ForegroundColor;
                Console.ForegroundColor = _foreground.Value;
            }
        }

        public void Dispose() {
            if ( _prevForeground.HasValue ) {
                Console.ForegroundColor = _prevForeground.Value;
            }
        }
    }
}
