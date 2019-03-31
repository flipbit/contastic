using System;

namespace Contastic.Utils
{
    public class ConsoleScreen : IScreen
    {
        public int Width => Console.WindowWidth;

        public int Left => Console.CursorLeft;
    }
}
