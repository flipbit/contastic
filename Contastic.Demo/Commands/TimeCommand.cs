using System;

namespace Contastic.Demo.Commands
{
    public class TimeCommand : Command<TimeCommand.Options>
    {
        [Flag("time")]
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
            Console.WriteLine("Current time: {0}", DateTime.Now.TimeOfDay);

            return 0;
        }
    }
}
