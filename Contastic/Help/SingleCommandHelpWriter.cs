using System;
using System.Collections.Generic;
using System.Linq;
using Contastic.Binding;
using Contastic.Models;
using Contastic.Utils;

namespace Contastic
{
    /// <summary>
    /// Displays help text for a single <see cref="ICommand"/>.
    /// </summary>
    public class SingleCommandHelpWriter : IHelpWriter
    {
        private readonly AppInfo info;

        public SingleCommandHelpWriter()
        {
            info = new AppInfo();
        }

        public void Write(Invocation invocation)
        {
            var binding = invocation.BindResults.FirstOrDefault();

            if (binding != null)
            {
                Write(binding);
            }
            else
            {
                Console.WriteLine("No commands registered.");
            }
        }

        public void Write(CanBindResult binding)
        {
            if (binding.HasUnknownArguments)
            {
                WriteUnknownArguments(binding);
                return;
            }

            WriteNoArgumentsSpecificCommand(binding);
        }


        private void WriteNoArgumentsSpecificCommand(CanBindResult binding)
        {
            WriteApplicationHeader();

            Console.WriteLine();
            Console.WriteLine("USAGE");
            #if NETSTANDARD2_0
            Console.WriteLine($"  {info.Prompt} dotnet .\\{info.FileName} {binding.ToHelpText()}");
            #else
            Console.WriteLine($"  > {info.FileName} {binding.ToHelpText()}");
            #endif
            Console.WriteLine();

            var table = new TextTable();
            table.ColumnSeparator = "  ";
            table.Align(Align.Right, Align.Left);

            var arguments = binding.AllArguments;

            if (arguments.Any())
            {
                table.AddHeader("ARGUMENTS");

                foreach (var argument in binding.AllArguments)
                {
                    table.AddRow($"  <{argument.Name}>", argument.Description);
                }
            }

            var options = binding.AllOptions;

            if (options.Any())
            {
                table.AddRow("");
                table.AddHeader("OPTIONS");

                foreach (var option in options)
                {
                    table.AddRow($"  {option.SwitchString}", option.Description);
                }
            }

            table.WriteConsole();
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

        private void WriteSingleCommandNoArguments(Invocation invocation)
        {
            var binding = invocation.BindResults.First();

            var details = new AppInfo();
            var verbs = ""; //GetVerbs(binding);
            var options = GetOptions(binding, new TextTable());

            Console.WriteLine(details.Description);
            Console.WriteLine();
            Console.WriteLine("USAGE");
            Console.WriteLine($"  $ dotnet .\\{details.FileName}{verbs}");
            Console.WriteLine();

            if (options.Any())
            {
                Console.WriteLine("OPTIONS");

                foreach (var option in options)
                {
                    Console.WriteLine(option);
                }
            }

            Console.WriteLine();
        }

        private List<string> GetOptions(CanBindResult binding, TextTable table)
        {
            var options = new List<string>();

            var allOptions = new List<IOption>();

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
