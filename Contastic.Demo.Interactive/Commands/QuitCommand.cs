using System;

namespace Contastic.Demo.Interactive.Commands
{
    public class QuitCommand : Command<QuitCommand.Options>
    {
        [Flag("quit")]
        public class Options
        {
        }

        /// <summary>
        /// Executes the specified options.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        public override int Execute(Options options)
        {
            Environment.Exit(0);

            return 0;
        }
    }
}
