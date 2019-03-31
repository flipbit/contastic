using System;
using System.Threading.Tasks;
using Contastic.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace Contastic.Demo.Interactive.Core
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var services = new ServiceCollection();
            services.AddTransientCommand<PullCommand>();
            services.AddTransientCommand<PushCommand>();
            services.AddTransientCommand<CopyCommand>();
            services.AddContastic();

            var provider = services.BuildServiceProvider();

            var runner = provider.GetService<IInteractiveRunner>();

            var exitCode = await runner.Run(args);

            Environment.Exit(exitCode);
        }
    }
}
