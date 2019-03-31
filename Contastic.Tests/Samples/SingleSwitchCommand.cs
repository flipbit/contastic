using System.Threading.Tasks;

namespace Contastic.Samples
{
    public class SingleSwitchCommand : ICommand
    {
        [Option(LongName = "single-switch", ShortName = 's')]
        public bool Flag { get; set; }

        public Task<int> Execute()
        {
            throw new System.NotImplementedException();
        }
    }
}
