using System;
using System.Collections.Generic;
using System.Text;

namespace Contastic.Utils
{
    public static class Hyphonator
    {
        public static void Write(this IWriter writer, string message, int maxLength, int margin)
        {
            var enumerator = new Enumerator(message);

            while (enumerator.Empty == false)
            {
                var next = enumerator.Next();

                if (string.IsNullOrWhiteSpace(next))
                {
                    var lineRemaining = writer.Screen.Width - writer.Screen.Left;
                    var nextWhiteSpace = enumerator.LengthTillNextWhiteSpace;

                    if (enumerator.Remaining >= lineRemaining)
                    {
                        writer.Write(Environment.NewLine);
                        continue;
                    }

                    if (nextWhiteSpace >= lineRemaining)
                    {
                        writer.Write(Environment.NewLine);
                        continue;
                    }
                }

                writer.Write(next);
            }
        }

        private class Enumerator
        {
            private readonly string contents;
            private int index = 0;

            public Enumerator(string contents)
            {
                this.contents = contents;
            }

            public string Next()
            {
                var next = contents.Substring(index, 1);

                index++;

                return next;
            }

            public int LengthTillNextWhiteSpace
            {
                get
                {
                    for (var i = 0; i < contents.Length - index; i++)
                    {
                        if (string.IsNullOrWhiteSpace(contents.Substring(index + i, 1)))
                        {
                            return i;
                        }
                    }

                    return -1;
                }
            }

            public int Remaining => contents.Length - index;

            public bool Empty => index >= contents.Length;
        }
    }
}
