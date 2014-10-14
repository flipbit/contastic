using System;
using System.Collections.Generic;
using System.Reflection;

namespace Contastic
{
    /// <summary>
    /// Class to bind a <see cref="ParameterList"/> onto a given object
    /// </summary>
    public class ParameterBinder
    {
        /// <summary>
        /// Binds the specified paramters.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public T Bind<T>(ParameterList parameters) where T : new()
        {
            var target = new T();

            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            // Bind properties
            target = BindProperties(target, properties, parameters);

            return target;
        }

        /// <summary>
        /// Binds the given parameters onto the properties of the given target.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target">The object.</param>
        /// <param name="properties">The properties.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        /// <exception cref="Contastic.BindingException">Can't Bind Object</exception>
        private T BindProperties<T>(T target, IEnumerable<PropertyInfo> properties, ParameterList parameters)
        {
            foreach (var property in properties)
            {
                var attribute = GetCustomAttribute<ParameterAttribute>(property);

                if (attribute == null) continue;

                // Skip empty parameters
                if (string.IsNullOrEmpty(attribute.Name)) continue;

                // Set value
                if (parameters.Contains(attribute.Name))
                {
                    var value = parameters.GetValue(attribute.Name);

                    property.SetValue(target, value, null);

                    continue;
                }

                // Set default value
                if (!attribute.Required && !string.IsNullOrEmpty(attribute.Default))
                {
                    property.SetValue(target, attribute.Default, null);

                    continue;
                }

                // Skip optional parameters
                if (!attribute.Required) continue;

                throw new BindingException("Can't Bind Object", parameters, target);
            }

            return target;
        }

        private static T GetCustomAttribute<T>(PropertyInfo propertyInfo)
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
