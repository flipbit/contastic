using System.Collections.Generic;

namespace Contastic
{
    /// <summary>
    /// List collection of <see cref="ParameterItem"/> objects.
    /// </summary>
    public class ParameterList : List<ParameterItem>
    {
        /// <summary>
        /// Determines whether this instance contains a parameter with the given name.
        /// </summary>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="caseInsensitveSearch">if set to <c>true</c> [case insensitve search].</param>
        /// <returns></returns>
        public bool Contains(string parameterName, bool caseInsensitveSearch)
        {
            return GetParameter(parameterName, caseInsensitveSearch) != null;
        }

        /// <summary>
        /// Gets the parameter value with the given name.
        /// </summary>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="caseInsensitveSearch">if set to <c>true</c> [case insensitve search].</param>
        /// <returns></returns>
        public string GetValue(string parameterName, bool caseInsensitveSearch)
        {
            var result = string.Empty;

            var item = GetParameter(parameterName, caseInsensitveSearch);

            if (item != null)
            {
                result = item.Value;
            }

            return result;
        }

        /// <summary>
        /// Gets the parameter with the given name.
        /// </summary>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="caseInsensitveSearch">if set to <c>true</c> [case insensitve search].</param>
        /// <returns></returns>
        public ParameterItem GetParameter(string parameterName, bool caseInsensitveSearch)
        {
            foreach (var item in this)
            {
                if (string.Compare(item.Name, parameterName, caseInsensitveSearch) == 0)
                {
                    return item;
                }
            }

            return null;
        }
    }
}
