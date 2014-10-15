using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Contastic
{
    /// <summary>
    /// Base class that assembles and executes a collection of <see cref="ICommand"/> objects
    /// </summary>
    public abstract class CommandRunner
    {
        /// <summary>
        /// Gets the commands.
        /// </summary>
        /// <value>
        /// The commands.
        /// </value>
        public IList<ICommand> Commands { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandRunner"/> class.
        /// </summary>
        public CommandRunner()
        {
            Commands = new List<ICommand>();
        }

        /// <summary>
        /// Runs the specified arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public virtual int Run(string[] args)
        {
            var sb = new StringBuilder();

            foreach (var arg in args)  
            {
                if (sb.Length == 0) sb.Append(" ");

                sb.Append(arg);
            }

            return Run(sb.ToString());
        }

        /// <summary>
        /// Runs the specified argument.
        /// </summary>
        /// <param name="arg">The argument.</param>
        /// <returns></returns>
        public virtual int Run(string arg)
        {
            foreach (var command in Commands)
            {
                if (!command.CanExecute(arg)) continue;

                return command.Execute(arg);
            }

            return -1;
        }

        public void Initialize(Assembly assembly)
        {
            var finder = new CommandFinder();

            var commands = finder.Find(assembly);
            var sorted = finder.SortCommands(commands);

            foreach (var command in sorted)
            {
                Commands.Add(command);
            }
        }
    }
}
