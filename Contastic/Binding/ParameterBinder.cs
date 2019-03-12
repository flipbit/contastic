using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Contastic.Models;

namespace Contastic.Binding
{
    internal class ParameterBinder
    {
        public CanBindResult CanBind<T>(IList<Parameter> parameters) where T : class, ICommand, new()
        {
            return CanBind(parameters, typeof(T));
        }

        public CanBindResult CanBind(IList<Parameter> parameters, Type type)
        {
            var result = new CanBindResult();

            return Bind(parameters, result, type);
        }

        public BindResult<T> Bind<T>(IList<Parameter> parameters) where T : class, ICommand, new()
        {
            var result = new BindResult<T>();

            result.Value = new T();

            return Bind(parameters, result, typeof(T)) as BindResult<T>;
        }

        public BindResult Bind(IList<Parameter> parameters, Type type)
        {
            var result = new BindResult();

            result.Value = Activator.CreateInstance(type);

            return Bind(parameters, result, type) as BindResult;
        }
        
        private CanBindResult Bind(IList<Parameter> parameters, CanBindResult result, Type type)
        {
            result.Type = type;


            var verbNames = type
                .GetCustomAttributes(typeof(VerbsAttribute), true)
                .Cast<VerbsAttribute>()
                .Select(v => v.Names)
                .SelectMany(v => v);

            foreach (var verb in verbNames)
            {
                var matches = parameters
                    .Where(p => p.Type == ParameterType.Verb)
                    .Where(p => p.LongName == verb)
                    .ToList();

                if (matches.Any())
                {
                    result.Bound = true;
                    continue;
                }

                result.Bound = false;
            }

            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            BindArguments(parameters, properties, result);

            BindSwitches(parameters, properties, result);

            BindVerbs(parameters, type, result);

            if (result.UnboundVerbs.Count == 0)
            {
                BindUnnamedArguments(parameters, properties, result);
            }

            if (result.UnboundArguments.Any(a => a.Required))
            {
                result.Bound = false;
            }

            if (result.UnboundSwitches.Any(s => s.Required))
            {
                result.Bound = false;
            }

            if (result.UnboundVerbs.Any())
            {
                result.Bound = false;
            }

            if (result.Bound == false)
            {
                if (result is BindResult boundResult)
                {
                    boundResult.Value = null;
                }
            }

            foreach (var parameter in parameters)
            {
                if (parameter.Bound == false)
                {
                    if (parameter.Type == ParameterType.Verb)
                    {
                        result.UnknownVerbs.Add(new Verb
                        {
                            Name = parameter.LongName,
                            Index = parameter.Index
                        });
                    }

                    if (parameter.Type == ParameterType.Argument)
                    {
                        result.UnknownArguments.Add(new Argument
                        {
                            ShortName = parameter.ShortName,
                            LongName = parameter.LongName,
                            Index = parameter.Index
                        });
                    }
                }
            }


            return result;
        }

        private static void BindSwitches(IList<Parameter> parameters, PropertyInfo[] properties, CanBindResult result)
        {
            foreach (var property in properties)
            {
                var switches = property.GetCustomAttributes(typeof(SwitchAttribute), true).Cast<SwitchAttribute>();

                foreach (var switchAttribute in switches)
                {
                    var matches = parameters
                        //.Where(p => p.Type == ParameterType.Argument)
                        .Where(p => p.LongName == switchAttribute.LongName)
                        .ToList();

                    if (matches.Any())
                    {
                        var first = matches.First();

                        result.Bound = true;
                        result.AddBoundSwitch(first);

                        if (result is BindResult boundResult)
                        {
                            property.SetValue(boundResult.Value, true);
                        }

                        continue;
                    }

                    matches = parameters
                        //.Where(p => p.Type == ParameterType.Argument)
                        .Where(p => p.ShortName == switchAttribute.ShortName)
                        .ToList();

                    if (matches.Any())
                    {
                        var first = matches.First();

                        result.Bound = true;
                        result.AddBoundSwitch(first);

                        if (result is BindResult boundResult)
                        {
                            property.SetValue(boundResult.Value, true);
                        }

                        continue;
                    }

                    if (switchAttribute.Required == false)
                    {
                        result.Bound = true;
                    }

                    result.AddUnboundSwitch(switchAttribute);
                }
            }
        }

        private static void BindArguments(IList<Parameter> parameters, PropertyInfo[] properties, CanBindResult result)
        {
            foreach (var property in properties)
            {
                var arguments = property.GetCustomAttributes(typeof(ParameterAttribute), true).Cast<ParameterAttribute>();

                foreach (var argument in arguments)
                {
                    var matches = parameters
                        .Where(p => p.Type == ParameterType.Argument)
                        .Where(p => p.LongName == argument.LongName)
                        .ToList();

                    if (matches.Any())
                    {
                        var first = matches.First();

                        result.Bound = true;

                        var value = first.Value;
                        if (string.IsNullOrEmpty(value))
                        {
                            var nextVerb = parameters
                                .Where(p => p.Type == ParameterType.Verb)
                                .FirstOrDefault(p => p.Index == first.Index + 1);

                            if (nextVerb != null)
                            {
                                nextVerb.Bound = true;
                                value = nextVerb.LongName;
                            }
                        }

                        result.AddBoundArgument(property, first, value, false, 0);

                        if (result is BindResult boundResult)
                        {
                            var convertedValue = Convert.ChangeType(value, property.PropertyType);
                            property.SetValue(boundResult.Value, convertedValue);
                        }

                        continue;
                    }

                    matches = parameters
                        .Where(p => p.Type == ParameterType.Argument)
                        .Where(p => p.ShortName == argument.ShortName)
                        .ToList();

                    if (matches.Any())
                    {
                        var first = matches.First();

                        result.Bound = true;

                        var value = first.Value;
                        if (string.IsNullOrEmpty(value))
                        {
                            var nextVerb = parameters
                                .Where(p => p.Type == ParameterType.Verb)
                                .FirstOrDefault(p => p.Index == first.Index + 1);

                            if (nextVerb != null)
                            {
                                nextVerb.Bound = true;
                                value = nextVerb.LongName;
                            }
                        }

                        result.AddBoundArgument(property, first, value, false, 0);

                        if (result is BindResult boundResult)
                        {
                            var convertedValue = Convert.ChangeType(value, property.PropertyType);
                            property.SetValue(boundResult.Value, convertedValue);
                        }

                        continue;
                    }

                    if (argument.Required == false)
                    {
                        result.Bound = true;
                    }

                    result.AddUnboundArgument(property, argument);
                }
            }
        }

        private static void BindUnnamedArguments(IList<Parameter> parameters, PropertyInfo[] properties, CanBindResult result)
        {
            foreach (var property in properties)
            {
                var arguments = property.GetCustomAttributes(typeof(UnnamedParameterAttribute), true).Cast<UnnamedParameterAttribute>();

                foreach (var argument in arguments)
                {
                    var match = parameters
                        .Where(p => p.Type == ParameterType.Verb)
                        .Where(p => p.Bound == false)
                        .ElementAtOrDefault(argument.Order);

                    if (match != null)
                    {
                        result.Bound = true;
                        result.AddBoundArgument(property, match, match.LongName, true, argument.Order);

                        if (result is BindResult boundResult)
                        {
                            var value = Convert.ChangeType(match.LongName, property.PropertyType);
                            property.SetValue(boundResult.Value, value);
                        }

                        continue;
                    }

                    result.AddUnboundArgument(property, argument);
                }
            }
        }

        private static void BindVerbs(IList<Parameter> parameters, Type type, CanBindResult result)
        {
            var verbs = type
                .GetCustomAttributes(typeof(VerbAttribute), true)
                .Cast<VerbAttribute>();

            var counter = 0;

            foreach (var verb in verbs)
            {
                var match = parameters
                    .Where(p => p.Type == ParameterType.Verb)
                    .Where(p => p.LongName == verb.Name)
                    .FirstOrDefault(p => p.Bound == false);

                if (match != null)
                {
                    match.Bound = true;
                    result.Bound = true;
                    result.AddBoundVerb(match, counter);
                    continue;
                }

                result.AddUnboundVerb(verb, counter);

                counter++;
            }
        }
    }
}
