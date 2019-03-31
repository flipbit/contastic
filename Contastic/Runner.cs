using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contastic.Binding;
using Contastic.Commands;
using Contastic.Models;
using Contastic.Parsing;

namespace Contastic
{
    public class Runner : IRunner
    {
        public IList<Type> Commands { get; private set; }

        internal TokenParser Parser { get; set; }

        internal TokenBinder Binder { get; set; }

        public IHelpWriter HelpWriter { get; set; }

        public IInvoker Invoker
        {
            get => Binder.Invoker;
            set => Binder.Invoker = value;
        }

        public Runner()
        {
            Commands = new List<Type>();
            Parser = new TokenParser();
            Binder = new TokenBinder();
            Invoker = new ActivatorInvoker();
            HelpWriter = new SingleCommandHelpWriter();
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
                    if (bindResult.Value is HelpCommand helpCommand)
                    {
                        helpCommand.Invocation = invocation;
                    }

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

        public void Register(Type type)
        {
            Commands.Add(type); 
        }
    }
}
