using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Contastic.Models;

namespace Contastic.Commands
{
    [Verb]
    [Description("Prints command usage information")]
    public class HelpCommand : ICommand
    {
        private readonly InteractiveHelpWriter helpWriter;

        public Invocation Invocation { get; set; }

        [Argument]
        [Description("The command to provide help on")]
        public string Command { get; set; }

        public HelpCommand()
        {
            helpWriter = new InteractiveHelpWriter();
        }

        public Task<int> Execute()
        {
            if (string.IsNullOrWhiteSpace(Command))
            {
                helpWriter.WriteCommands(Invocation);
            }
            else
            {
                helpWriter.WriteCommand(Invocation, Command);
            }

            return Task.FromResult(0);
        }
    }
}
