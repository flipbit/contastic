using System.Threading.Tasks;

namespace Contastic.Samples
{
    [Verb("copy")]
    public class CopyCommand : ICommand
    {
        [Parameter(LongName = "user", ShortName = 'u')]
        public string UserName { get; set; }

        [Parameter(LongName = "password", ShortName = 'p')]
        public string Password { get; set; }

        [UnnamedParameter(Order = 0, Required = true)]
        public string Source { get; set; }

        [UnnamedParameter(Order = 1, Required = true)]
        public string Destination { get; set; }

        public Task<int> Execute()
        {
            throw new System.NotImplementedException();
        }
    }
}
