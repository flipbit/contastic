using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Contastic
{
    /// <summary>
    /// Parser to take an input string and convert it into a collection of <see cref="ParameterItem"/> objects.
    /// </summary>
    public class ParameterParser
    {
        /// <summary>
        /// Gets the default parsing options options.
        /// </summary>
        /// <value>
        /// The options.
        /// </value>
        public ParameterParserOptions Options { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParameterParser"/> class.
        /// </summary>
        public ParameterParser()
        {
            // Set default options
            Options = new ParameterParserOptions();

            // Default allowable switches
            Options.ParameterSwitches.Add("-");
            Options.ParameterSwitches.Add("--");
            Options.ParameterSwitches.Add("/");
        }

        /// <summary>
        /// Parses the specified input into a <see cref="ParameterItem"/> list.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public ParameterList Parse(string input)
        {
            var results = new ParameterList();

            var matches = Regex.Split(input, @"(?<!""\b[^""]*)\s+(?![^""]*\b"")");

            ParameterItem current = null;

            for (var i = 0; i < matches.Length; i++)
            {
                var match = matches[i];

                if (IsASwitch(match))
                {
                    if (current != null) results.Add(current);

                    current = new ParameterItem();
                    current.Name = RemoveSwitch(match);
                }
                else if (current != null)
                {
                    if (match.StartsWith(@"""")) match = match.Substring(1);
                    if (match.EndsWith(@"""")) match = match.Substring(0, match.Length - 1);

                    current.Values.Add(match);
                }
            }

            if (current != null)
            {
                results.Add(current);
            }

            return results;
        }

        /// <summary>
        /// Determines whether the specified input is a parameter or a value.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public bool IsASwitch(string input)
        {
            return IsASwitch(input, Options.ParameterSwitches);
        }

        /// <summary>
        /// Determines whether the specified input is a parameter or a value.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        public bool IsASwitch(string input, ParameterParserOptions options)
        {
            return IsASwitch(input, options.ParameterSwitches);
        }

        /// <summary>
        /// Determines whether the specified input is a parameter or a value.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="switches">The switches.</param>
        /// <returns></returns>
        public bool IsASwitch(string input, IList<string> switches)
        {
            if (string.IsNullOrEmpty(input)) return false;

            foreach (var @switch in switches)
            {
                if (input.StartsWith(@switch)) return true;
            }

            return false;
        }

        /// <summary>
        /// Removes the parameter switch from the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public string RemoveSwitch(string input)
        {
            return RemoveSwitch(input, Options);
        }

        /// <summary>
        /// Removes the parameter switch from the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        public string RemoveSwitch(string input, ParameterParserOptions options)
        {
            return RemoveSwitch(input, options.ParameterSwitches);
        }

        /// <summary>
        /// Removes the parameter switch from the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="switches">The switches.</param>
        /// <returns></returns>
        public string RemoveSwitch(string input, IList<string> switches)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;

            var result = input;

            foreach (var @switch in switches)
            {
                if (input.StartsWith(@switch))
                {
                    result = input.Substring(@switch.Length);
                }
            }

            return result;
        }
    }
}
