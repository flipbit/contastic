using System.Text;

namespace Contastic.Models
{
    public class Parameter
    {
        private readonly StringBuilder name;
        private readonly StringBuilder value;

        public Parameter()
        {
            name = new StringBuilder();
            value = new StringBuilder();
        }

        public int Index { get; set; }

        public string LongName => name.ToString();

        public char ShortName { get; set; }

        public string Value => value.ToString();

        public ParameterType Type { get; set; }

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
