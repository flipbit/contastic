using System;
using System.Text;

namespace Contastic.Utils
{
    public class StringWriter : IWriter
    {
        private readonly StringBuilder sb = new StringBuilder();
        private readonly VirtualScreen screen;

        public StringWriter()
        {
            screen = new VirtualScreen();
        }

        public StringWriter(int width)
        {
            screen = new VirtualScreen
            {
                Width = width
            };
        }

        public void Write(char value)
        {
            sb.Append(value);

            switch (value)
            {
                case '\n':
                    screen.Left = 0;
                    break;
                case '\r':
                    // Nothing
                    break;
                default:
                    screen.Left++;
                    break;
            }

            if (screen.Width > 0 && screen.Left >= screen.Width)
            {
                screen.Left = 0;
                sb.Append(Environment.NewLine);
            }
        }

        public IScreen Screen => screen;

        public override string ToString()
        {
            return sb.ToString();
        }
    }
}