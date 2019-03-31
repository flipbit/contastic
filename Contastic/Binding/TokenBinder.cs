using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Contastic.Commands;
using Contastic.Models;

namespace Contastic.Binding
{
    internal class TokenBinder
    {
        public IInvoker Invoker { get; set; }

        public TokenBinder()
        {
            Invoker = new ActivatorInvoker();
        }

        public CanBindResult CanBind<T>(IList<Token> parameters) where T : class, ICommand, new()
        {
            return CanBind(parameters, typeof(T));
        }

        public CanBindResult CanBind(IList<Token> parameters, Type type)
        {
            var result = new CanBindResult();

            return Bind(parameters, result, type);
        }

        public BindResult<T> Bind<T>(IList<Token> tokens) where T : class, ICommand, new()
        {
            var result = new BindResult<T>();

            result.Value = (T) Invoke(typeof(T));

            return Bind(tokens, result, typeof(T)) as BindResult<T>;
        }

        public BindResult Bind(IList<Token> tokens, Type type)
        {
            var result = new BindResult();

            result.Value = Invoke(type);

            if (result.Value == null)
            {
                throw new ContasticException($"Unable to create type: {type.FullName}");
            }

            return Bind(tokens, result, type) as BindResult;
        }
        
        private CanBindResult Bind(IList<Token> tokens, CanBindResult result, Type type)
        {
            result.Type = type;

            //BindMultipleVerbs(tokens, result, type);

            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            BindOptions(tokens, properties, result);

            BindVerbs(tokens, type, result);

            if (result.UnboundVerbs.Count == 0)
            {
                BindUnnamedArguments(tokens, properties, result);
            }

            if (result.UnboundArguments.Any(s => s.Required))
            {
                result.Bound = false;
            }

            if (result.UnboundOptions.Any(a => a.Required))
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

            foreach (var token in tokens)
            {
                if (token.Bound) continue;

                if (token.Type == TokenType.Argument)
                {
                    result.UnknownVerbs.Add(new Verb
                    {
                        Name = token.LongName,
                        Index = token.Index
                    });
                }

                if (token.Type == TokenType.Option)
                {
                    result.UnknownOptions.Add(new Option
                    {
                        ShortName = token.ShortName,
                        LongName = token.LongName,
                        Index = token.Index
                    });
                }
            }


            return result;
        }

        private static void BindMultipleVerbs(IList<Token> parameters, CanBindResult result, Type type)
        {
            var verbNames = type
                .GetCustomAttributes(typeof(VerbsAttribute), true)
                .Cast<VerbsAttribute>()
                .Select(v => v.Names)
                .SelectMany(v => v);

            foreach (var verb in verbNames)
            {
                var matches = parameters
                    .Where(p => p.Type == TokenType.Argument)
                    .Where(p => p.LongName == verb)
                    .ToList();

                if (matches.Any())
                {
                    result.Bound = true;
                    continue;
                }

                result.Bound = false;
            }
        }

        private static void BindOptions(IList<Token> parameters, PropertyInfo[] properties, CanBindResult result)
        {
            foreach (var property in properties)
            {
                var arguments = property.GetCustomAttributes(typeof(OptionAttribute), true).Cast<OptionAttribute>();

                foreach (var argument in arguments)
                {
                    var matches = parameters
                        .Where(p => p.Type == TokenType.Option)
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
                                .Where(p => p.Type == TokenType.Argument)
                                .FirstOrDefault(p => p.Index == first.Index + 1);

                            if (nextVerb != null)
                            {
                                nextVerb.Bound = true;
                                value = nextVerb.LongName;
                            }
                        }

                        result.AddBoundOption(property, first, value, false, 0);

                        if (result is BindResult boundResult)
                        {
                            if (property.PropertyType == typeof(bool))
                            {
                                property.SetValue(boundResult.Value, true);
                            }
                            else
                            {
                                var convertedValue = Convert.ChangeType(value, property.PropertyType);
                                property.SetValue(boundResult.Value, convertedValue);
                            }
                        }

                        continue;
                    }

                    matches = parameters
                        .Where(p => p.Type == TokenType.Option)
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
                                .Where(p => p.Type == TokenType.Argument)
                                .FirstOrDefault(p => p.Index == first.Index + 1);

                            if (nextVerb != null)
                            {
                                nextVerb.Bound = true;
                                value = nextVerb.LongName;
                            }
                        }

                        result.AddBoundOption(property, first, value, false, 0);

                        if (result is BindResult boundResult)
                        {
                            if (property.PropertyType == typeof(bool))
                            {
                                property.SetValue(boundResult.Value, true);
                            }
                            else
                            {
                                var convertedValue = Convert.ChangeType(value, property.PropertyType);
                                property.SetValue(boundResult.Value, convertedValue);
                            }
                        }

                        continue;
                    }

                    if (argument.Required == false)
                    {
                        result.Bound = true;
                    }

                    result.AddUnboundOption(property, argument);
                }
            }
        }

        private static void BindUnnamedArguments(IList<Token> tokens, PropertyInfo[] properties, CanBindResult result)
        {
            foreach (var property in properties)
            {
                var arguments = property.GetCustomAttributes(typeof(ArgumentAttribute), true).Cast<ArgumentAttribute>();

                foreach (var argument in arguments)
                {
                    var match = tokens
                        .Where(p => p.Type == TokenType.Argument)
                        .Where(p => p.Bound == false)
                        .ElementAtOrDefault(argument.Order);

                    if (match != null)
                    {
                        result.Bound = true;
                        result.AddBoundArgument(argument, match, property);

                        if (result is BindResult boundResult)
                        {
                            var value = Convert.ChangeType(match.LongName, property.PropertyType);
                            property.SetValue(boundResult.Value, value);
                        }

                        continue;
                    }

                    result.AddUnboundArgument(argument, property);
                }
            }
        }

        private static void BindVerbs(IList<Token> parameters, Type type, CanBindResult result)
        {
            var verbs = type
                .GetCustomAttributes(typeof(VerbAttribute), true)
                .Cast<VerbAttribute>();

            var counter = 0;

            foreach (var verb in verbs)
            {
                var verbName = verb.Name;
                if (string.IsNullOrEmpty(verbName))
                {
                    verbName = type.Name.ToLowerInvariant();
                    if (verbName.EndsWith("command"))
                    {
                        verbName = verbName.Substring(0, verbName.Length - 7);
                    }
                }

                var match = parameters
                    .Where(p => p.Type == TokenType.Argument)
                    .FirstOrDefault(p => p.Bound == false);

                if (match != null && match.LongName == verbName)
                {
                    match.Bound = true;
                    result.Bound = true;
                    result.AddBoundVerb(match, counter);
                    continue;
                }

                result.AddUnboundVerb(verbName, counter);

                counter++;
            }
        }

        private object Invoke(Type type)
        {
            if (type == typeof(ExitCommand))
            {
                return new ExitCommand();
            }

            if (type == typeof(HelpCommand))
            {
                return new HelpCommand();
            }

            return Invoker.Invoke(type);
        }
    }
}
