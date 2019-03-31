using System.Threading.Tasks;

namespace Contastic.Samples
{
    [Verb("copy")]
    public class CopyCommand : ICommand
    {
        [Option(LongName = "user", ShortName = 'u')]
        public string UserName { get; set; }

        [Option(LongName = "password", ShortName = 'p')]
        public string Password { get; set; }

        [Argument(Order = 0, Required = true)]
        public string Source { get; set; }

        [Argument(Order = 1, Required = true)]
        public string Destination { get; set; }

        public Task<int> Execute()
        {
            throw new System.NotImplementedException();
        }
    }
}
