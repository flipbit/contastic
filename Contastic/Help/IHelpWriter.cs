using Contastic.Models;

namespace Contastic
{
    /// <summary>
    /// A help writer displays feedback to the user on command arguments,
    /// options, descriptions and invocation errors.
    /// </summary>
    public interface IHelpWriter
    {
        /// <summary>
        /// Write the help text for the given <see cref="Invocation"/>.
        /// </summary>
        void Write(Invocation invocation);
    }
}