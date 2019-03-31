using System;
using System.Threading.Tasks;

namespace Contastic
{
    /// <summary>
    /// Represents a class capable of running command lines.
    /// </summary>
    public interface IRunner
    {
        /// <summary>
        /// The invoker class used to create instances of <see cref="ICommand"/>
        /// objects when they have been bound.
        /// </summary>
        IInvoker Invoker { get; set; }

        /// <summary>
        /// The command line to run
        /// </summary>
        Task<int> Run(string[] args);

        /// <summary>
        /// The command line to run
        /// </summary>
        Task<int> Run(string args);

        /// <summary>
        /// Registers the given command type with this instance
        /// </summary>
        void Register<T>() where T : ICommand;

        /// <summary>
        /// Registers the given command type with this instance
        /// </summary>
        void Register(Type type);
    }
}