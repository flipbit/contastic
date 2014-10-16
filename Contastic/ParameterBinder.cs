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
        /// Gets the options.
        /// </summary>
        /// <value>
        /// The options.
        /// </value>
        public Options Options { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParameterBinder"/> class.
        /// </summary>
        public ParameterBinder()
        {
            Options = Options.Defaults();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParameterBinder"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public ParameterBinder(Options options)
        {
            Options = options;
        }

        /// <summary>
        /// Binds the specified input to an object of the given type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public T Bind<T>(string input) where T : new()
        {
            var parser = new ParameterParser(Options);

            var parameters = parser.Parse(input);

            return Bind<T>(parameters);
        }
        
        /// <summary>
        /// Binds the specified paramters to an object of the given type.
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

            // Bind flags
            target = BindFlags(target, properties, parameters);
            target = BindFlags(target, parameters);

            return target;
        }

        /// <summary>
        /// Tries to bind the given input onto the given target.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input">The input.</param>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        public bool TryBind<T>(string input, out T target) where T : new()
        {
            try
            {
                target = Bind<T>(input);

                return true;
            }
            catch (BindingException)
            {
                target = default(T);

                return false;
            }
        }

        /// <summary>
        /// Tries to bind the given input onto the given target.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameters">The parameters.</param>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        public bool TryBind<T>(ParameterList parameters, out T target) where T : new()
        {
            try
            {
                target = Bind<T>(parameters);

                return true;
            }
            catch (BindingException)
            {
                target = default(T);

                return false;
            }
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
                var attribute = AttributeFinder.GetCustomAttribute<ParameterAttribute>(property);

                if (attribute == null) continue;

                // Set empty names to those of the decorated property
                if (string.IsNullOrEmpty(attribute.Name))
                {
                    attribute.Name = property.Name.ToLower();
                }

                // Set value
                if (parameters.Contains(attribute.Name, Options.CaseInsensitve))
                {
                    var parameter = parameters.GetParameter(attribute.Name, Options.CaseInsensitve);

                    SetObjectProperty(target, property, parameter.Value);

                    continue;
                }

                // Set default value
                if (!attribute.Required && !string.IsNullOrEmpty(attribute.Default))
                {
                    SetObjectProperty(target, property, attribute.Default);

                    continue;
                }

                // Skip optional parameters
                if (!attribute.Required) continue;

                var message = string.Format("Couldn't bind to object, missing required parameter: {0}", attribute.Name);

                throw new BindingException(message, parameters, target);
            }

            return target;
        }

        /// <summary>
        /// Binds the given flags onto the properties of the given target.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target">The object.</param>
        /// <param name="properties">The properties.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        /// <exception cref="Contastic.BindingException">Can't Bind Object</exception>
        private T BindFlags<T>(T target, IEnumerable<PropertyInfo> properties, ParameterList parameters)
        {
            foreach (var property in properties)
            {
                var attribute = AttributeFinder.GetCustomAttribute<FlagAttribute>(property);

                if (attribute == null) continue;

                // Skip empty parameters
                if (string.IsNullOrEmpty(attribute.Name)) continue;

                // Skip non booleans
                if (property.PropertyType != typeof(Boolean)) continue;

                // Set value
                if (parameters.Contains(attribute.Name, Options.CaseInsensitve))
                {
                    SetObjectProperty(target, property, true);

                    continue;
                }

                // Skip non-required flags
                if (!attribute.Required) continue;
                
                var message = string.Format("Couldn't bind to object, missing required flag: {0}", attribute.Name);

                throw new BindingException(message, parameters, target);
            }

            return target;
        }

        private T BindFlags<T>(T target, ParameterList parameters)
        {
            var attribute = AttributeFinder.GetCustomAttribute<FlagAttribute>(target.GetType());

            if (attribute != null)
            {
                // Set value
                if (!parameters.Contains(attribute.Name, Options.CaseInsensitve))
                {
                    throw new BindingException("Missing Flag: " + attribute.Name, parameters, target);
                }
            }

            return target;
        }


        private static void SetObjectProperty<T>(T target, PropertyInfo property, object value)
        {
            object convertedValue;

            try
            {
                convertedValue = Convert.ChangeType(value, property.PropertyType);
            }
            catch (FormatException ex)
            {
                var message = string.Format("Could not convert '{0}' to type {1}", value, property.PropertyType);

                throw new BindingException(message, target, ex);
            }

            property.SetValue(target, convertedValue, null);
        }
    }
}
