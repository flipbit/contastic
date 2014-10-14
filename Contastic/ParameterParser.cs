using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Contastic
{
    /// <summary>
    /// Parser to take an input string and convert it into a collection of <see cref="Parameter"/> objects.
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
        /// Parses the specified input into a <see cref="Parameter"/> list.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public IList<Parameter> Parse(string input)
        {
            var result = new List<Parameter>();

            var matches = Regex.Split(input, @"(?<!""\b[^""]*)\s+(?![^""]*\b"")");

            for (var i = 0; i < matches.Length; i++)
            {
                var match = matches[i];

                var parameter = new Parameter();

                parameter.Name = RemoveSwitch(match, Options);
                parameter.Value = matches[++i];

                result.Add(parameter);
            }

            return result;
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
            if (string.IsNullOrWhiteSpace(input)) return false;

            return switches.Any(input.StartsWith);
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
