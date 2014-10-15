using System.Reflection;

namespace Contastic
{
    /// <summary>
    /// Extension methods for <see cref="PropertyInfo"/> objects.
    /// </summary>
    public static class PropertyInfoExtensions
    {
        /// <summary>
        /// Gets the first custom attribute from the property.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyInfo">The property information.</param>
        /// <returns></returns>
        public static T GetCustomAttribute<T>(this PropertyInfo propertyInfo)
        {
            var result = default(T);

            var attributes = propertyInfo.GetCustomAttributes(typeof(T), false);

            if (attributes.Length > 0)
            {
                result = (T)attributes[0];
            }

            return result;
        }
    }
}
