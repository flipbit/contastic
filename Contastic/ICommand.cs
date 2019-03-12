using System.Threading.Tasks;

namespace Contastic
{
    /// <summary>
    /// Interface for an individual command
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// Executes the specified arguments.
        /// </summary>
        /// <returns></returns>
        Task<int> Execute();
    }
}
