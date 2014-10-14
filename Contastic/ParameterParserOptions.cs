using System.Collections.Generic;

namespace Contastic
{
    /// <summary>
    /// Configuration object for the <see cref="ParameterParser"/> class.
    /// </summary>
    public class ParameterParserOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ParameterParserOptions"/> class.
        /// </summary>
        public ParameterParserOptions()
        {
            ParameterSwitches = new List<string>();    
        }

        /// <summary>
        /// Gets the list of valid parameter switches.
        /// </summary>
        /// <value>
        /// The parameter switches.
        /// </value>
        public IList<string> ParameterSwitches { get; private set; }
    }
}
