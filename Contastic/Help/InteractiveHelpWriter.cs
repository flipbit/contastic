using System;
using System.Collections.Generic;
using System.Linq;
using Contastic.Binding;
using Contastic.Models;
using Contastic.Utils;

namespace Contastic
{
    public class InteractiveHelpWriter : IHelpWriter
    {
        public void Write(Invocation invocation)
        {
            if (invocation.HasAnyBoundVerbs)
            {
                var bindResult = invocation.BestMatchingBinding;

                Write(bindResult);
            }
            else
            {
                WriteUnknownCommand(invocation);
            }
        }

        /// <summary>
        /// Displays usage information for a single command
        /// </summary>
        public void WriteCommand(Invocation invocation, string command)
        {
            var bindResult = invocation
                .BindResults
                .FirstOrDefault(br => br.Verbs == command);

            if (bindResult != null)
            {
                Write(bindResult);
            }
        }

        /// <summary>
        /// Displays all available commands
        /// </summary>
        public void WriteCommands(Invocation invocation)
        {
            WriteApplicationHeader();
            Console.WriteLine();
            Console.WriteLine("COMMANDS");

            var commands = invocation
                .BindResults
                .ToList();

            foreach (var command in commands)
            {
                Console.Write($"  {command.Verbs}");

                if (!string.IsNullOrEmpty(command.Description))
                {
                    Console.WriteLine($" - {command.Description}");
                }
                else
                {
                    Console.WriteLine($" - {command.Type.FullName}");
                }
            }

            Console.WriteLine();
        }

        /// <summary>
        /// No matching commands found
        /// </summary>
        public void WriteUnknownCommand(Invocation invocation)
        {
            Console.WriteLine($"Unknown command: {invocation.Args}");
        }

        public void Write(CanBindResult bindResult)
        {
            // Only command verb specified: show usage
            if (bindResult.HasNoArguments)
            {
                WriteUsage(bindResult);
                return;
            }

            // Extra arguments?
            // TODO: missing args
            if (bindResult.HasUnknownArguments)
            {
                WriteUnknownArguments(bindResult);
                return;
            }

            Console.WriteLine("EH?");
        }

        public void WriteUsage(CanBindResult bindResult)
        {
            var table = new TextTable {ColumnSeparator = "  "};
            table.Align(Align.Right, Align.Left);

            table.AddHeader(bindResult.Description);
            table.AddHeader("");

            var arguments = bindResult
                .BoundOptions.Where(a => a.Unnamed)
                .Concat(bindResult.UnboundOptions.Where(a => a.Unnamed))
                .OrderBy(a => a.Name)
                .ToList();

            if (arguments.Any())
            {
                table.AddHeader("ARGUMENTS");
                foreach (var argument in arguments)
                {
                    table.AddRow($"  [{argument.Name}]", argument.Description);
                }

            }

            GetOptions(bindResult, table);

            table.WriteConsole();
            Console.WriteLine();
        }

        private void WriteUnknownArguments(CanBindResult binding)
        {
            Console.WriteLine("Unknown arguments:");

            foreach (var verb in binding.UnknownVerbs)
            {
                Console.WriteLine($"  {verb.Name}");
            }

            foreach (var argument in binding.UnknownOptions)
            {
                if (string.IsNullOrWhiteSpace(argument.LongName) == false)
                {
                    Console.WriteLine($"  --{argument.LongName}");
                }
                else
                {
                    Console.WriteLine($"  -{argument.ShortName}");
                }
            }

            Console.WriteLine();
            Console.WriteLine("Run --help for usage.");
        }

        private List<string> GetOptions(CanBindResult binding, TextTable table)
        {
            var options = new List<string>();

            var allOptions = new List<IOption>();
            allOptions.AddRange(binding.BoundOptions.Where(a => a.Unnamed == false));
            allOptions.AddRange(binding.UnboundOptions.Where(a => a.Unnamed == false));

            allOptions = allOptions
                .OrderBy(o => o.ShortName)
                .ThenBy(o => o.LongName)
                .ToList();

            if (allOptions.Any())
            {
                table.AddHeader("");
                table.AddHeader("OPTIONS");
            }

            foreach (var option in allOptions)
            {
                var switchString = "  ";

                if (option.ShortName != '\0')
                {
                    switchString += $"-{option.ShortName}";
                }

                if (string.IsNullOrEmpty(option.LongName) == false)
                {
                    if (string.IsNullOrWhiteSpace(switchString) == false)
                    {
                        switchString += ", ";
                    }

                    switchString += $"--{option.LongName}";
                }

                // TODO: Property Name / Description
                table.AddRow(switchString, option.Description);

                if (string.IsNullOrWhiteSpace(switchString) == false)
                {
                    options.Add(switchString);
                }
            }

            return options;
        }

        private void WriteApplicationHeader()
        {
            var info = new AppInfo();

            if (string.IsNullOrEmpty(info.Title) == false)
            {
                Console.WriteLine(info.Title);
            }

            if (string.IsNullOrEmpty(info.Description) == false)
            {
                Console.WriteLine(info.Description);
            }

            Console.WriteLine($"Version: {info.Version}  Built: {info.BuildDate:d MMM yyyy}");
        }
    }
}
