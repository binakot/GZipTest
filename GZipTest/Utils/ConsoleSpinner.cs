using System;
using System.Threading;

namespace GZipTest.Utils
{
    /// <summary>
    /// Simple console spinner.
    /// Long duration operation's indicator.
    /// </summary>
    public sealed class ConsoleSpinner : IDisposable
    {
        private readonly int _cursorLeftPosition;
        private readonly int _cursorTopPosition;
        private int _counter;

        public ConsoleSpinner()
        {
            _cursorLeftPosition = Console.CursorLeft;
            _cursorTopPosition = Console.CursorTop;

            Console.CursorVisible = false;
        }

        public void Dispose()
        {
            Console.CursorVisible = true;
        }

        public void Turn()
        {
            _counter++;
            switch (_counter % 4)
            {
                case 0:
                    Console.Out.Write("/");
                    _counter = 0;
                    break;
                case 1:
                    Console.Out.Write("-");
                    break;
                case 2:
                    Console.Out.Write("\\");
                    break;
                case 3:
                    Console.Out.Write("|");
                    break;
            }

            Console.SetCursorPosition(_cursorLeftPosition, _cursorTopPosition);
            Thread.Sleep(100);
        }
    }
}