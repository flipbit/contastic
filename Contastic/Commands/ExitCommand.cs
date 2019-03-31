using System.Threading.Tasks;

namespace Contastic.Commands
{
    [Verb("exit")]
    public class ExitCommand : ICommand
    {
        public static bool ExitRequested { get; set; }

        public Task<int> Execute()
        {
            ExitRequested = true;

            return Task.FromResult(0);
        }
    }
}
