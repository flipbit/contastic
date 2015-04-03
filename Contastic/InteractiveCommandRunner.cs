using System;

namespace Contastic
{
    /// <summary>
    /// REPL type command runner
    /// </summary>
    public class InteractiveCommandRunner : CommandRunner
    {
        /// <summary>
        /// Runs the specified argument.
        /// </summary>
        /// <param name="arg">The argument.</param>
        /// <returns></returns>
        public override int Run(string arg)
        {
            var firstRun = true;

            while (true)
            {
                if (firstRun && !string.IsNullOrEmpty(arg))
                {
                    base.Run("-" + arg);

                    firstRun = false;
                }

                Prompt();

                var command = "-" + ReadUserInput();

                var returnCode = base.Run(command);

                if (returnCode == -1)
                {
                    OnUnknownInput(command.Substring(1));
                }
            }

            return 0;
        }

        /// <summary>
        /// Reads the user input.
        /// </summary>
        /// <returns></returns>
        public virtual string ReadUserInput()
        {
            return Console.ReadLine();
        }

        /// <summary>
        /// Displays the user input prompt.
        /// </summary>
        /// <returns></returns>
        public virtual void Prompt()
        {
            Console.Write(">");
        }

        /// <summary>
        /// Fired when the runner receives some unknown input
        /// </summary>
        /// <param name="unknownInput">The unknown command.</param>
        public virtual void OnUnknownInput(string unknownInput)
        {
            Console.WriteLine("Unknown command: {0}", unknownInput);            
        }
    }
}
