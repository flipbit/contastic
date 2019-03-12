using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using Contastic.Models;

namespace Contastic.Binding
{
    internal class CanBindResult
    {
        public List<Verb> BoundVerbs { get; }

        public List<Argument> BoundArguments { get; }

        public List<Switch> BoundSwitches { get; }

        public List<Verb> UnboundVerbs { get; }

        public List<Argument> UnboundArguments { get; }

        public List<Switch> UnboundSwitches { get; }

        public List<Verb> UnknownVerbs { get; }

        public List<Argument> UnknownArguments { get; }

        public bool Bound { get; set; }

        public Type Type { get; set; }

        public int TotalBound => BoundVerbs.Count + BoundArguments.Count + BoundSwitches.Count;

        public int TotalUnbound => UnboundVerbs.Count + UnboundArguments.Count + UnboundSwitches.Count;

        public bool HasUnknownArguments => UnknownVerbs.Count + UnknownArguments.Count > 0;

        public bool HasNoArguments => BoundArguments.Count + BoundSwitches.Count == 0;

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

        public string UnnamedArguments
        {
            get
            {
                var arguments = BoundArguments
                    .Concat(UnboundArguments)
                    .Where(a => a.Unnamed)
                    .Where(a => a.PropertyInfo != null)
                    .OrderBy(a => a.Order)
                    .Select(a => $"[{a.PropertyInfo.Name}]");

                return string.Join(" ", arguments);
            }
        }

        public CanBindResult()
        {
            BoundVerbs = new List<Verb>();
            BoundArguments = new List<Argument>();
            BoundSwitches = new List<Switch>();

            UnboundVerbs = new List<Verb>();
            UnboundArguments = new List<Argument>();
            UnboundSwitches = new List<Switch>();

            UnknownVerbs = new List<Verb>();
            UnknownArguments = new List<Argument>();
        }

        public void AddBoundVerb(Parameter parameter, int order)
        {
            BoundVerbs.Add(new Verb
            {
                Index = parameter.Index,
                Name = parameter.LongName,
                Order = order
            });
        }

        public void AddUnboundVerb(VerbAttribute verb, int order)
        {
            UnboundVerbs.Add(new Verb
            {
                Name = verb.Name,
                Index = -1,
                Order = order
            });
        }

        public void AddBoundArgument(PropertyInfo property, Parameter parameter, string value, bool  unnamed, int order)
        {
            BoundArguments.Add(new Argument
            {
                PropertyInfo = property,
                LongName = parameter.LongName,
                ShortName = parameter.ShortName,
                Value = value,
                Index = parameter.Index,
                Order = order,
                Unnamed = unnamed
            });
        }

        public void AddUnboundArgument(PropertyInfo property, ParameterAttribute parameter)
        {
            UnboundArguments.Add(new Argument
            {
                PropertyInfo = property,
                LongName = parameter.LongName,
                ShortName = parameter.ShortName,
                Required = parameter.Required,
                Index = 0
            });
        }

        public void AddUnboundArgument(PropertyInfo property, UnnamedParameterAttribute parameter)
        {
            UnboundArguments.Add(new Argument
            {
                PropertyInfo = property,
                Required = parameter.Required,
                Order = parameter.Order,
                Index = 0,
                Unnamed = true
            });
        }

        public void AddBoundSwitch(Parameter parameter)
        {
            BoundSwitches.Add(new Switch
            {
                LongName = parameter.LongName,
                ShortName = parameter.ShortName,
                Index = parameter.Index
            });
        }

        public void AddUnboundSwitch(SwitchAttribute attribute)
        {
            UnboundSwitches.Add(new Switch
            {
                LongName = attribute.LongName,
                ShortName = attribute.ShortName,
                Required = attribute.Required,
                Index = 0
            });
        }
    }
}
