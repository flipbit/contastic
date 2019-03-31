using System;

namespace Contastic.Utils
{
    public class ConsoleWriter : IWriter
    {
        public ConsoleWriter()
        {
            Screen = new ConsoleScreen();
        }

        public void Write(char value)
        {
            Console.Write(value); 
        }

        public IScreen Screen { get; }
    }
}