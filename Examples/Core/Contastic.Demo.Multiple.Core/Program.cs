using System;
using System.Threading.Tasks;
using Contastic.Commands;

namespace Contastic
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var runner = new Runner();
            runner.Register<PushCommand>();
            runner.Register<PullCommand>();

            var result = await runner.Run(args);

            Environment.Exit(result);        }
    }
}
