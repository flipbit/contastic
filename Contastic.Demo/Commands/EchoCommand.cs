using System;

namespace Contastic.Demo.Commands
{
    public class EchoCommand : Command<EchoCommand.Options>
    {
        public class Options
        {
            [Parameter("echo", Required = true)]
            public string Message { get; set; }
        }

        /// <summary>
        /// Executes the specified options.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        public override int Execute(Options options)
        {
            Console.WriteLine(options.Message);

            return 0;
        }
    }
}
