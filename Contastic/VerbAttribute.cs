using System;

namespace Contastic
{
    /// <summary>
    /// Binds a verb onto a command 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class VerbAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VerbAttribute"/> class.
        /// </summary>
        public VerbAttribute(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Gets or sets the name of the verb.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; }
    }
}
