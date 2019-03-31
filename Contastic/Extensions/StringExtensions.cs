using System;
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
        /// <param name="seperator">The separator to place between each string.</param>
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
        
        /// <summary>
        /// Determines whether the specified value is numeric.
        /// </summary>
        public static bool IsNumeric(this string value)
        {
            return double.TryParse(value, out _);
        }

        public static string Repeat(this string value, int times)
        {
            var result = string.Empty;

            for (var i = 0; i < times; i++)
            {
                result += value;
            }

            return result;
        }
        
        public static string TrimTo(this string value, int length)
        {
            return TrimTo(value, length, string.Empty);
        }

        public static string TrimTo(this string value, int length, string overrunIndicator)
        {
            var result = value;

            if (!string.IsNullOrEmpty(value))
            {
                if (value.Length > length)
                {
                    result = value.Substring(0, length - overrunIndicator.Length) + overrunIndicator;
                }
            }

            return result;
        }
        
        /// <summary>
        /// Splits the specified value using the given separator.
        /// </summary>
        public static IList<string> Split(this string value, string separator, StringSplitOptions options = StringSplitOptions.None)
        {
            var results = new List<string>();

            if (!string.IsNullOrWhiteSpace(value))
            {
                results.AddRange(value.Split(new[] { separator }, options));
            }

            return results;
        }
    }
}
