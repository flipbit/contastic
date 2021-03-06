﻿using System.Collections.Generic;

namespace Contastic
{
    /// <summary>
    /// Configuration object for the Contastic library.
    /// </summary>
    public class Options
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Options"/> class.
        /// </summary>
        public Options()
        {
            ParameterSwitches = new List<string>();
        }

        /// <summary>
        /// Gets the list of valid option switches.
        /// </summary>
        /// <value>
        /// The option switches.
        /// </value>
        public IList<string> ParameterSwitches { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether option matching is case insensitve.
        /// </summary>
        /// <value>
        ///   <c>true</c> if case insensitve; otherwise, <c>false</c>.
        /// </value>
        public bool CaseInsensitve { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to strip smart quotes when parsing input.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [strip smart quotes]; otherwise, <c>false</c>.
        /// </value>
        public bool StripSmartQuotes { get; set; }

        /// <summary>
        /// Gets the default options.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public static Options Defaults()
        {
            var options = new Options();

            // Default allowable switches
            options.ParameterSwitches.Add("-");
            options.ParameterSwitches.Add("--");
            options.ParameterSwitches.Add("/");

            // Default to case insensitive
            options.CaseInsensitve = true;

            // Replace smart quotes with regular quotes
            options.StripSmartQuotes = true;

            return options;
        }
    }
}
