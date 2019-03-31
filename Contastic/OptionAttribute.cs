using System;

namespace Contastic
{
    /// <summary>
    /// Decorates a property with the target for an incoming command line <see cref="ParameterItem"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class OptionAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string LongName { get; set; }

        public char ShortName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="OptionAttribute"/> is required.
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
