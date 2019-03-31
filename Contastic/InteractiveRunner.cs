using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Contastic.Commands;

namespace Contastic
{
    public class InteractiveRunner : IInteractiveRunner
    {
        private readonly IRunner runner;

        public InteractiveRunner()
        {
            runner = new Runner
            {
                HelpWriter = new InteractiveHelpWriter()
            };
        }

        public InteractiveRunner(IEnumerable<Type> commands, IInvoker invoker) : this()
        {
            if (commands != null)
            {
                foreach (var command in commands)
                {
                    runner.Register(command);
                }

                runner.Register(typeof(HelpCommand));
                runner.Register(typeof(ExitCommand));
            }

            runner.Invoker = invoker;
        }

        public async Task<int> Run(string[] args)
        {
            ExitCommand.ExitRequested = false;

            while (ExitCommand.ExitRequested == false)
            {
                Console.Write(">");
                var input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input) == false)
                {
                    await runner.Run(input);
                }
            }

            return 0;
        }
    }
}
