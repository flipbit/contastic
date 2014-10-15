using System;

namespace Contastic
{
    /// <summary>
    /// Flag parameter that sets a boolean property on an object to true
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = false)]
    public class FlagAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FlagAttribute"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public FlagAttribute(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Gets or sets the name of the flag.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="FlagAttribute"/> is required.
        /// </summary>
        /// <value>
        ///   <c>true</c> if required; otherwise, <c>false</c>.
        /// </value>
        public bool Required { get; set; }
    }
}
