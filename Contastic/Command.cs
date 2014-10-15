namespace Contastic
{
    /// <summary>
    /// Base command that maps command line arguments onto an object of type <see cref="T"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Command<T> : ICommand where T : class, new()
    {
        /// <summary>
        /// Determines whether this instance can execute the specified arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <returns></returns>
        public virtual bool CanExecute(string args)
        {
            var binder = new ParameterBinder();

            T options;

            return binder.TryBind(args, out options);
        }

        /// <summary>
        /// Executes the specified arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <returns></returns>
        public int Execute(string args)
        {
            var binder = new ParameterBinder();

            var options = binder.Bind<T>(args);
            
            return Execute(options);
        }

        /// <summary>
        /// Executes the specified options.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        public abstract int Execute(T options);
    }
}
