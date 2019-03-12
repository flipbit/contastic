using System;
using System.Collections.Generic;
using System.Linq;
using Contastic.Binding;

namespace Contastic.Models
{
    /// <summary>
    /// Holds the result of attempting to bind a command line onto a set of
    /// <see cref="ICommand"/> instances.
    /// </summary>
    internal class Invocation
    {
        public Invocation(string args)
        {
            Args = args;
            BindResults = new List<CanBindResult>();
        }

        public string Args { get; }

        /// <summary>
        /// Determines whether the binding was successful
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Determines the bound type, if any
        /// </summary>
        public Type CommandType { get; set; }

        /// <summary>
        /// Holds the results of the binding
        /// </summary>
        public List<CanBindResult> BindResults { get; }

        public CanBindResult BestMatchingBinding
        {
            get
            {
                if (BindResults.Count == 0) return null;

                var result = BindResults
                    .Where(b => b.UnboundVerbs.Count == 0)
                    .OrderByDescending(b => b.BoundVerbs.Count)
                    .ThenByDescending(b => b.BoundArguments.Count)
                    .ThenByDescending(b => b.BoundSwitches.Count)
                    .FirstOrDefault();

                return result;
            }
        }
    }
}