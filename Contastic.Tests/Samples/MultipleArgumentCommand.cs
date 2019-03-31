using System.Threading.Tasks;

namespace Contastic.Samples
{
    public class MultipleArgumentCommand : ICommand
    {
        [Option(LongName = "one", ShortName = 'o', Required = true)]
        public string One { get; set; }

        [Option(LongName = "two", ShortName = 't')]
        public string Two { get; set; }

        [Option(LongName = "three", ShortName = '3')]
        public string Three { get; set; }

        public Task<int> Execute()
        {
            throw new System.NotImplementedException();
        }
    }
}
