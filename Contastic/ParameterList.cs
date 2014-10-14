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
        /// <returns></returns>
        public bool Contains(string parameterName)
        {
            return GetParameter(parameterName) != null;
        }

        public string GetValue(string parameterName)
        {
            var result = string.Empty;

            var item = GetParameter(parameterName);

            if (item != null)
            {
                result = item.Value;
            }

            return result;
        }

        public ParameterItem GetParameter(string parameterName)
        {
            foreach (var item in this)
            {
                if (item.Name == parameterName) return item;
            }

            return null;
        }
    }
}
