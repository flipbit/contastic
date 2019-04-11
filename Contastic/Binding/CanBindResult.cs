using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using Contastic.Models;

namespace Contastic.Binding
{
    public class CanBindResult
    {
        public List<Verb> BoundVerbs { get; }

        public List<Argument> BoundArguments { get; }

        public List<Option> BoundOptions { get; }

        public List<Verb> UnboundVerbs { get; }

        public List<Argument> UnboundArguments { get; set; }

        public List<Option> UnboundOptions { get; }

        public List<Verb> UnknownVerbs { get; }

        public List<Option> UnknownOptions { get; }

        public List<Argument> UnknownArguments { get; set; }

        public bool Bound { get; set; }

        public Type Type { get; set; }

        public int TotalBound => BoundVerbs.Count + BoundArguments.Count + BoundOptions.Count;

        public int TotalUnbound => UnboundVerbs.Count + UnboundArguments.Count + UnboundOptions.Count;

        public bool HasUnknownArguments => UnknownVerbs.Count + UnknownArguments.Count + UnknownOptions.Count > 0;

        public bool HasNoArguments => BoundOptions.Count + BoundArguments.Count == 0;

        public string Verbs
        {
            get
            {
                var verbs = BoundVerbs
                    .Concat(UnboundVerbs)
                    .OrderBy(v => v.Order)
                    .Select(v => v.Name);

                return string.Join(" ", verbs);
            }
        }

        public string Description
        {
            get
            {
                var description = string.Empty;

                var attribute = Type.GetCustomAttribute<DescriptionAttribute>();
                if (attribute != null)
                {
                    description = attribute.Description;
                }
                else
                {
                    description = Type.Name;
                }

                return description;
            }
        }

        public IList<Argument> AllArguments
        {
            get
            {
                return BoundArguments
                    .Concat(UnboundArguments)
                    .OrderBy(a => a.Name)
                    .ToList();
            }
        }

        public IList<Option> AllOptions
        {
            get
            {
                return BoundOptions
                    .Concat(UnboundOptions)
                    .OrderBy(o => o.Name)
                    .ToList();
            }
        }

        public CanBindResult()
        {
            BoundVerbs = new List<Verb>();
            BoundArguments = new List<Argument>();
            BoundOptions = new List<Option>();

            UnboundVerbs = new List<Verb>();
            UnboundArguments = new List<Argument>();
            UnboundOptions = new List<Option>();

            UnknownVerbs = new List<Verb>();
            UnknownArguments = new List<Argument>();
            UnknownOptions = new List<Option>();
        }

        internal void AddBoundVerb(Token parameter, int order)
        {
            BoundVerbs.Add(new Verb
            {
                Index = parameter.Index,
                Name = parameter.LongName,
                Order = order
            });
        }

        public void AddUnboundVerb(string verb, int order)
        {
            UnboundVerbs.Add(new Verb
            {
                Name = verb,
                Index = -1,
                Order = order
            });
        }

        internal void AddBoundOption(PropertyInfo property, OptionAttribute option, Token token, string value)
        {
            BoundOptions.Add(new Option
            {
                PropertyInfo = property,
                LongName = token.LongName,
                ShortName = token.ShortName,
                Value = value,
                Index = token.Index,
                //Order = option.,
                Required = option.Required
            });
        }

        internal void AddUnboundOption(PropertyInfo property, OptionAttribute option)
        {
            UnboundOptions.Add(new Option
            {
                PropertyInfo = property,
                LongName = option.LongName,
                ShortName = option.ShortName,
                Required = option.Required,
                Index = 0
            });
        }

        internal void AddUnboundOption(PropertyInfo property, ArgumentAttribute parameter)
        {
            UnboundOptions.Add(new Option
            {
                PropertyInfo = property,
                Required = parameter.Required,
                Order = parameter.Order,
                Index = 0,
            });
        }

        internal void AddUnboundArgument(ArgumentAttribute attribute, PropertyInfo propertyInfo)
        {
            var name = attribute.Name;
            if (string.IsNullOrEmpty(name)) name = propertyInfo.Name;

            UnboundArguments.Add(new Argument
            {
                Name = name,
                Required = attribute.Required,
                Index = attribute.Order,
                PropertyInfo = propertyInfo
            });
        }

        internal void AddBoundArgument(ArgumentAttribute attribute, Token token, PropertyInfo propertyInfo)
        {
            var name = attribute.Name;
            if (string.IsNullOrEmpty(name)) name = propertyInfo.Name;

            BoundArguments.Add(new Argument
            {
                Name = name,
                Required = attribute.Required,
                Index = attribute.Order,
                Value = token.Value,
                PropertyInfo = propertyInfo
            });
        }

        public string ToHelpText()
        {
            var sb = new StringBuilder();

            foreach (var argument in AllArguments)
            {
                if (sb.Length > 0) sb.Append(" ");

                sb.Append("<");
                sb.Append(argument.Name.ToLower());
                sb.Append(">");
            }

            foreach (var option in AllOptions)
            {
                if (sb.Length > 0) sb.Append(" ");

                if (string.IsNullOrEmpty(option.LongName) == false)
                {
                    if (option.Required == false) sb.Append("[");
                    sb.Append("--");
                    sb.Append(option.LongName);
                    if (option.Required == false) sb.Append("]");
                }

                else if (option.ShortName != '\0')
                {
                    if (option.Required == false) sb.Append("[");
                    sb.Append("-");
                    sb.Append(option.ShortName);
                    if (option.Required == false) sb.Append("]");
                }
            }

            return sb.ToString();
        }
    }
}
