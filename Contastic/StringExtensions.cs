using System.Collections.Generic;
using System.Text;

namespace Contastic
{
    public static class StringExtensions
    {
        /// <summary>
        /// Joins the specified strings.
        /// </summary>
        /// <param name="strings">The strings.</param>
        /// <param name="seperator">The seperator to place between each string.</param>
        /// <returns></returns>
        public static string Join(this IEnumerable<string> strings, string seperator = " ")
        {
            var sb = new StringBuilder();

            if (strings != null)
            {
                foreach (var s in strings)
                {
                    if (string.IsNullOrEmpty(s)) continue;

                    if (sb.Length > 0) sb.Append(seperator);

                    sb.Append(s);
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Strips the quotes from an input string.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static string StripQuotes(this string input)
        {
            if (string.IsNullOrEmpty(input)) input = string.Empty;
            if (input.StartsWith(@"""")) input = input.Substring(1);
            if (input.EndsWith(@"""")) input = input.Substring(0, input.Length - 1);

            return input;
        }
    }
}
