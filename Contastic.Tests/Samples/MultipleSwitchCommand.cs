using System.Threading.Tasks;

namespace Contastic.Samples
{
    public class MultipleSwitchCommand : ICommand
    {
        [Switch(LongName = "one", ShortName = 'o', Required = true)]
        public bool One { get; set; }

        [Switch(LongName = "two", ShortName = 't')]
        public bool Two { get; set; }

        [Switch(LongName = "three", ShortName = '3')]
        public bool Three { get; set; }

        public Task<int> Execute()
        {
            throw new System.NotImplementedException();
        }
    }
}
