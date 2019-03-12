using System;
using System.Threading.Tasks;

namespace Contastic.Commands
{
    [Verb("push")]
    public class PushCommand : ICommand
    {
        [UnnamedParameter(Order = 0, Required = true)]
        public string Destination { get; set; }

        public Task<int> Execute()
        {
            Console.WriteLine("Push");

            return Task.FromResult(0);
        }
    }
}
