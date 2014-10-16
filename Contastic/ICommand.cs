namespace Contastic
{
    /// <summary>
    /// Interface for an individual command
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// Determines whether this instance can execute the specified arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        bool CanExecute(string args, Options options);

        /// <summary>
        /// Executes the specified arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        int Execute(string args, Options options);
    }
}
