using System;

namespace Contastic.Demo.Interactive
{
    /// <summary>
    /// Demo REPL type application
    /// </summary>
    class Program
    {
        /// <summary>
        /// Main entry point into the application.
        /// </summary>
        /// <param name="args">The arguments.</param>
        static void Main(string[] args)
        {
            var runner = new InteractiveCommandRunner();

            runner.Initialize(typeof(Program).Assembly);

            Console.WriteLine("Contastic Demo");
            Console.WriteLine("");
            Console.WriteLine("Valid commands are: echo [message] and quit");

            runner.Run(args);
        }
    }
}
