using System;
using System.Threading.Tasks;
using Contastic.Commands;

namespace Contastic
{
    public class Program
    {
        private static async Task Main(string[] args)
        {
            var runner = new Runner();
            runner.Register<CopySingleCommand>();

            var exitCode = await runner.Run(args);

            Environment.Exit(exitCode);
        }
    }
}
