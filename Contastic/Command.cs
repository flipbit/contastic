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
        /// <param name="options">The options.</param>
        /// <returns></returns>
        public virtual bool CanExecute(string args, Options options)
        {
            var binder = new ParameterBinder(options);

            T parameters;

            return binder.TryBind(args, out parameters);
        }

        /// <summary>
        /// Executes the specified arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        public int Execute(string args, Options options)
        {
            var binder = new ParameterBinder(options);

            var paramters = binder.Bind<T>(args);
            
            return Execute(paramters);
        }

        /// <summary>
        /// Executes the specified paramters.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public abstract int Execute(T parameters);
    }
}
