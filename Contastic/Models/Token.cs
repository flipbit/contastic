using System.Text;

namespace Contastic.Models
{
    /// <summary>
    /// Represents a parsed command line token
    /// </summary>
    internal class Token
    {
        private readonly StringBuilder name;
        private readonly StringBuilder value;

        /// <summary>
        /// Creates a new <see cref="Token"/> instance.
        /// </summary>
        public Token()
        {
            name = new StringBuilder();
            value = new StringBuilder();
        }

        /// <summary>
        /// The index location of this <see cref="Token"/> on the command line.
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// The long name (e.g. "foo" for "--foo") of this token, if any.
        /// </summary>
        public string LongName => name.ToString();

        /// <summary>
        /// The short name (e.g. "f" for "-f") of this token, if any.
        /// </summary>
        public char ShortName { get; set; }

        /// <summary>
        /// The value of this token, if any.
        /// </summary>
        public string Value => value.ToString();

        /// <summary>
        /// The type of token
        /// </summary>
        public TokenType Type { get; set; }

        public bool Bound { get; set; }

        public bool HasName => !string.IsNullOrWhiteSpace(LongName) || ShortName != '\0';

        public void AppendName(string input)
        {
            name.Append(input);
        }

        public void AppendValue(string input)
        {
            value.Append(input);
        }
    }
}
