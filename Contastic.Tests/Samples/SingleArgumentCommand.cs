using System.Threading.Tasks;

namespace Contastic.Samples
{
    public class SingleArgumentCommand : ICommand
    {
        [Option(LongName = "single-argument", ShortName = 's')]
        public string Argument { get; set; }

        public Task<int> Execute()
        {
            throw new System.NotImplementedException();
        }
    }
}
