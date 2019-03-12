using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contastic.Binding;
using Contastic.Models;
using Contastic.Parsing;

namespace Contastic
{
    public class Runner
    {
        public IList<Type> Commands { get; private set; }

        public ParameterParser Parser { get; set; }

        internal ParameterBinder Binder { get; set; }

        internal HelpDescription HelpWriter { get; set; }

        public IInvoker Invoker { get; set; }

        public Runner()
        {
            Commands = new List<Type>();
            Parser = new ParameterParser();
            Binder = new ParameterBinder();
            Invoker = new ActivatorInvoker();
            HelpWriter = new HelpDescription();
        }

        public Runner(IServiceProvider serviceProvider)
        {

        }

        internal Invocation Invoke(string args)
        {
            var invocation = new Invocation(args);

            var parameters = Parser.Parse(args);

            foreach (var command in Commands)
            {
                var binding = Binder.CanBind(parameters, command);

                invocation.BindResults.Add(binding);
            }

            var match = invocation.BindResults
                .Where(b => b.Bound)
                .OrderByDescending(b => b.TotalBound)
                .ThenBy(b => b.TotalUnbound)
                .FirstOrDefault();

            if (match != null)
            {
                invocation.Success = true;
                invocation.CommandType = match.Type;
            }

            return invocation;
        }

        public async Task<int> Run(string[] args)
        {
            return await Run(args.Join());
        }

        public async Task<int> Run(string args)
        {
            var parameters = Parser.Parse(args);
            var invocation = Invoke(args);

            if (invocation.Success)
            {
                var bindResult = Binder.Bind(parameters, invocation.CommandType);

                if (bindResult.Bound && bindResult.Value is ICommand command)
                {
                    return await command.Execute();
                }
            }
            else
            {
                HelpWriter.Write(invocation);
            }

            return -1;
        }

        public void Register<T>() where T : ICommand
        {
            Commands.Add(typeof(T));
        }
    }
}
