﻿using System;

namespace Contastic
{
    /// <summary>
    /// Maps an unnamed parameter onto a class
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class UnnamedParameterAttribute : Attribute
    {
        /// <summary>
        /// The unnamed parameter order
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="UnnamedParameterAttribute"/> is required.
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
