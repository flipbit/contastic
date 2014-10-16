using System;
using System.Reflection;

namespace Contastic
{
    /// <summary>
    /// Finder class for getting attributes from objects.
    /// </summary>
    public static class AttributeFinder
    {
        /// <summary>
        /// Gets the first custom attribute from the type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static T GetCustomAttribute<T>(Type type)
        {
            var result = default(T);

            var attributes = type.GetCustomAttributes(typeof(T), false);

            if (attributes.Length > 0)
            {
                result = (T) attributes[0];
            }

            return result;
        }

        /// <summary>
        /// Gets the first custom attribute from the property.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyInfo">The property information.</param>
        /// <returns></returns>
        public static T GetCustomAttribute<T>(PropertyInfo propertyInfo)
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
