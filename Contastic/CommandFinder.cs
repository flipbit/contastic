using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Contastic
{
    /// <summary>
    /// Finder class to search assemblies for <see cref="ICommand"/> instances.
    /// </summary>
    public class CommandFinder
    {
        /// <summary>
        /// Finds all the classes implementing <see cref="ICommand"/> in the given assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns></returns>
        public IList<ICommand> Find(Assembly assembly)
        {
            var results = new List<ICommand>();

            var types = assembly.GetTypes();

            foreach (var type in types)
            {
                if (!ImplementsInterface(type, typeof(ICommand))) continue;

                var instance = (ICommand) Activator.CreateInstance(type);

                results.Add(instance);
            }

            return results;
        }

        /// <summary>
        /// Determines if the given type implements the given interface.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="interface">The interface.</param>
        /// <returns></returns>
        public bool ImplementsInterface(Type type, Type @interface)
        {
            var @interfaces = type.GetInterfaces();

            foreach (var candidate in interfaces)
            {
                if (candidate == @interface) return true;
            }

            if (type.BaseType != null)
            {
                return ImplementsInterface(type.BaseType, @interface);
            }

            return false;
        }

        /// <summary>
        /// Counts the number of parameters and flags on the given type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">Type must be of type Command<></exception>
        public int CountParameters(Type type)
        {
            var count = 0;

            if (type.BaseType.GetGenericTypeDefinition() != typeof(Command<>))
            {
                throw new ArgumentException("Type must be of type Command<>");
            }

            var optionsType = type.BaseType.GetGenericArguments()[0];

            var properties = optionsType.GetProperties();

            foreach (var property in properties)
            {
                var flagAttribute = property.GetCustomAttribute<FlagAttribute>();
                var parameterAttribute = property.GetCustomAttribute<ParameterAttribute>();

                if (flagAttribute != null) count++;
                if (parameterAttribute != null) count++;
            }

            return count;
        }

        private class ParameterCount
        {
            public ICommand Command { get; set; }
            public int Count { get; set; }
        }

        /// <summary>
        /// Sorts the given commands by the number of parameters they have.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public IList<ICommand> SortCommands(IList<ICommand> input)
        {
            var counts = new List<ParameterCount>();

            foreach (var command in input)
            {
                var count = new ParameterCount();
                count.Command = command;
                count.Count = CountParameters(command.GetType());

                counts.Add(count);
            }

            return counts
                .OrderByDescending(c => c.Count)
                .Select(c => c.Command)
                .ToList();
        }
    }
}
