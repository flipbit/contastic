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
        /// <returns></returns>
        bool CanExecute(string args);

        /// <summary>
        /// Executes the specified arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <returns></returns>
        int Execute(string args);
    }
}
