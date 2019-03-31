using System.Threading.Tasks;

namespace Contastic.Samples
{
    [Verb]
    public class HelpCommand : ICommand
    {
        [Argument]
        public string Command { get; set; }

        public Task<int> Execute()
        {
            throw new System.NotImplementedException();
        }
    }
}
