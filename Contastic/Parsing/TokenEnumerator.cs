using System.Text;

namespace Contastic.Parsing
{
    internal class TokenEnumerator
    {
        private readonly string input;
        private readonly int patternLength;

        private int currentLocation;

        private bool resetNextLine = true;

        public TokenEnumerator(string input)
        {
            this.input = input;

            if (string.IsNullOrEmpty(input))
            {
                patternLength = 0;
            }
            else
            {
                patternLength = input.Length;
            }

            currentLocation = 0;
        }

        public bool IsEmpty => currentLocation >= patternLength;

        public int Line { get; private set; }

        public int Character { get; private set; }

        public string Next()
        {
            if (IsEmpty) return string.Empty;

            if (resetNextLine)
            {
                Line++;
                Character = 1;
                resetNextLine = false;
            }

            var next = input.Substring(currentLocation, 1);

            currentLocation++;
            Character++;

            if (next == "\r" && IsEmpty == false)
            {
                var peek = input.Substring(currentLocation, 1);

                if (peek == "\n")
                {
                    next = "\n";
                    currentLocation++;
                    resetNextLine = true;
                }
            }
            else if (next == "\n")
            {
                resetNextLine = true;
            }

            return next;
        }

        public string Next(int length)
        {
            var sb = new StringBuilder();

            for (var i = 0; i < length; i++)
            {
                sb.Append(Next());
            }

            return sb.ToString();
        }

        public string Peek()
        {
            if (IsEmpty) return string.Empty;

            return input.Substring(currentLocation, 1);
        }

        public string Peek(int length)
        {
            if (IsEmpty) return string.Empty;

            var different = (currentLocation + length) - patternLength;
            if (different > 0) length -= different;

            if (length < 1) return string.Empty;

            return input.Substring(currentLocation, length);
        }

        public bool IsPeek(params string[] candidates)
        {
            if (candidates == null) return false;

            foreach (var candidate in candidates)
            {
                if (string.IsNullOrWhiteSpace(candidate)) continue;

                var peeked = Peek(candidate.Length);
                if (candidate == peeked) 
                {
                    return true;
                }
            }

            return false;
        }
    }
}
