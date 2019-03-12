using System;

namespace Contastic
{
    /// <summary>
    /// Flag parameter that sets a boolean property on an object to true
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = false)]
    public class SwitchAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the name of the flag.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string LongName { get; set; }

        public char ShortName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="SwitchAttribute"/> is required.
        /// </summary>
        /// <value>
        ///   <c>true</c> if required; otherwise, <c>false</c>.
        /// </value>
        public bool Required { get; set; }
    }
}
