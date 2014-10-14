using System;

namespace Contastic
{
    /// <summary>
    /// Decorates a property with the target for an incoming command line <see cref="Parameter"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class ParameterAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ParameterAttribute"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public ParameterAttribute(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ParameterAttribute"/> is required.
        /// </summary>
        /// <value>
        ///   <c>true</c> if required; otherwise, <c>false</c>.
        /// </value>
        public bool Required { get; set; }

        /// <summary>
        /// Gets or sets the default value for this attribute.
        /// </summary>
        /// <value>
        /// The default.
        /// </value>
        public string Default { get; set; }
    }
}
