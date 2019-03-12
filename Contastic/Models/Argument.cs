using System;
using System.Reflection;

namespace Contastic.Models
{
    public class Argument : IOption
    {
        public PropertyInfo PropertyInfo { get; set; }

        public string LongName { get; set; }

        public char ShortName { get; set; }

        public bool Required { get; set; }

        public string Value { get; set; }

        public int Index { get; set; }

        public int Order { get; set; }

        public bool Unnamed { get; set; }
    }
}
