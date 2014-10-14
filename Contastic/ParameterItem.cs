using System.Collections.Generic;
using System.Text;

namespace Contastic
{
    /// <summary>
    /// Represents a single command line parameter
    /// </summary>
    public class ParameterItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ParameterItem"/> class.
        /// </summary>
        public ParameterItem()
        {
            Values = new List<string>();
        }

        /// <summary>
        /// Gets or sets the name of the parameter.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets the parameter values.
        /// </summary>
        /// <value>
        /// The values.
        /// </value>
        public IList<string> Values { get; private set; }

        /// <summary>
        /// Gets the value of the paramter.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public string Value
        {
            get
            {
                var sb = new StringBuilder();

                foreach (var value in Values)
                {
                    if (sb.Length > 0) sb.Append(" ");

                    sb.Append(value);
                }

                return sb.ToString();
            }
            
        }


    }
}
