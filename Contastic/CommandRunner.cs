using System.Collections.Generic;
using System.Reflection;

namespace Contastic
{
    /// <summary>
    /// Base class that assembles and executes a collection of <see cref="ICommand"/> objects
    /// </summary>
    public class CommandRunner
    {
        /// <summary>
        /// Gets or sets the options.
        /// </summary>
        /// <value>
        /// The options.
        /// </value>
        public Options Options { get; set; }

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
            Options = Options.Defaults();
            Commands = new List<ICommand>();
        }

        /// <summary>
        /// Runs the specified arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public virtual int Run(string[] args)
        {
            return Run(args.Join());
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
                if (!command.CanExecute(arg, Options)) continue;

                return command.Execute(arg, Options);
            }

            return -1;
        }

        /// <summary>
        /// Initializes this instance with commands from the specified assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
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
