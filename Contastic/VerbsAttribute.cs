using System;
using System.Collections.Generic;

namespace Contastic
{
    /// <summary>
    /// Binds verbs onto a command 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class VerbsAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VerbsAttribute"/> class.
        /// </summary>
        public VerbsAttribute(params string[] names)
        {
            Names = names;
        }

        /// <summary>
        /// Gets or sets the name of the verb.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public IList<string> Names { get; }
    }
}
