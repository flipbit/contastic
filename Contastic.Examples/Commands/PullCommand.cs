using System;
using System.Threading.Tasks;

namespace Contastic.Commands
{
    [Verb("pull")]
    public class PullCommand : ICommand
    {
        [UnnamedParameter(Order = 0, Required = true)]
        public string Source { get; set; }

        public Task<int> Execute()
        {
            Console.WriteLine($"Pull: {Source}");

            return Task.FromResult(0);
        }
    }
}
