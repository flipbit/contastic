using System;

namespace Contastic
{
    /// <summary>
    /// Thrown when a <see cref="ParameterList"/> can't be bound to a given object
    /// </summary>
    public class BindingException : Exception
    {
        /// <summary>
        /// Gets the parameters that the <see cref="ParameterBinder"/> attempted to bind.
        /// </summary>
        /// <value>
        /// The parameters.
        /// </value>
        public ParameterList Parameters { get; private set; }

        /// <summary>
        /// Gets the target.
        /// </summary>
        /// <value>
        /// The target.
        /// </value>
        public object Target { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BindingException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="target">The target.</param>
        public BindingException(string message, ParameterList parameters, object target) : base(message)
        {
            Parameters = parameters;
            Target = target;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BindingException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="target">The value.</param>
        /// <param name="innerException">The inner exception.</param>
        public BindingException(string message, object target, Exception innerException) : base(message, innerException)
        {
            Target = target;
        }
    }
}
