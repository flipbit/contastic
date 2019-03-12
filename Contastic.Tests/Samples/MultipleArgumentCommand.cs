using System.Threading.Tasks;

namespace Contastic.Samples
{
    public class MultipleArgumentCommand : ICommand
    {
        [Parameter(LongName = "one", ShortName = 'o', Required = true)]
        public string One { get; set; }

        [Parameter(LongName = "two", ShortName = 't')]
        public string Two { get; set; }

        [Parameter(LongName = "three", ShortName = '3')]
        public string Three { get; set; }

        public Task<int> Execute()
        {
            throw new System.NotImplementedException();
        }
    }
}
