namespace Contastic.Models
{
    public class Switch : IOption
    {
        public string LongName { get; set; }

        public char ShortName { get; set; }

        public bool Required { get; set; }

        public int Index { get; set; }
    }
}
