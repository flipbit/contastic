using System;
using System.Collections.Generic;
using System.Linq;
using Contastic.Binding;
using Contastic.Models;
using Contastic.Utils;

namespace Contastic
{
    public class HelpWriter : IHelpWriter
    {
        public void Write(Invocation invocation)
        {
            // No Arguments
            if (string.IsNullOrWhiteSpace(invocation.Args))
            {
                // Single command
                if (invocation.BindResults.Count == 1)
                {
                    WriteSingleCommandNoArguments(invocation);
                }

                // Multiple commands
                if (invocation.BindResults.Count > 1)
                {
                    WriteMultipleCommandNoArguments(invocation);
                }

                return;
            }

            // Single command
            if (invocation.BindResults.Count == 1)
            {
                var binding = invocation.BindResults.First();

                if (binding.HasUnknownArguments)
                {
                    WriteUnknownArguments(binding);
                }
            }

            // Multiple commands
            if (invocation.BindResults.Count > 1)
            {
                // Help [Verb]
                var match = invocation.BestMatchingBinding;

                if (match != null)
                {
                    if (match.HasNoArguments)
                    {
                        WriteNoArgumentsSpecificCommand(match);
                    }
                    else
                    {
                        Console.WriteLine("HELP:command.miss");
                        
                    }
                }
                else
                {
                    WriteUnknownVerbs(invocation);
                }
            }

                // Arguments

                // Verb

                // No Verbs

                // Unknown Arguments
        }

        public void WriteInteractive(Invocation invocation)
        {
            if (invocation.HasAnyBoundVerbs)
            {
                var bindResult = invocation.BestMatchingBinding;

                WriteInteractive(bindResult);
            }
            else
            {
                WriteInteractiveUnknown(invocation);
            }
        }

        public void WriteInteractiveUnknown(Invocation invocation)
        {
            Console.WriteLine($"Unknown command: {invocation.Args}");
        }

        public void WriteInteractiveHelp(Invocation invocation)
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

        public void WriteInteractive(Invocation invocation, string command)
        {
            var bindResult = invocation
                .BindResults
                .FirstOrDefault(br => br.Verbs == command);

            if (bindResult != null)
            {
                WriteInteractive(bindResult);
            }
        }

        public void WriteInteractive(CanBindResult bindResult)
        {
        }

        private void WriteUnknownVerbs(Invocation invocation)
        {
            var details = new AppInfo();

            Console.WriteLine($"Unknown command: {invocation.Args}");
            Console.WriteLine();
            Console.WriteLine($@"Run ""dotnet .\{details.FileName} help"" for a list of commands.");
            
        }

        private void WriteNoArgumentsSpecificCommand(CanBindResult match)
        {
            var details = new AppInfo();

            Console.WriteLine(details.Description);
            Console.WriteLine();
            Console.WriteLine("USAGE");
            Console.WriteLine($"  $ dotnet .\\{details.FileName} {match.Verbs} {match.ToHelpText()}");
            Console.WriteLine();


        }

        private void WriteMultipleCommandNoArguments(Invocation invocation)
        {
            var details = new AppInfo();

            Console.WriteLine(details.Description);
            Console.WriteLine();
            Console.WriteLine("USAGE");
            Console.WriteLine($"  $ dotnet .\\{details.FileName} [COMMAND]");
            Console.WriteLine();

            Console.WriteLine("COMMANDS");

            var verbs = invocation
                .BindResults
                .Select(r => new { Verb = r.UnboundVerbs.FirstOrDefault(), r.Type })
                .Where(v => v.Verb != null)
                .GroupBy(g => g.Verb.Name);

            foreach (var verb in verbs)
            {
                var type = verb.FirstOrDefault();

                if (type != null)
                {
                    Console.WriteLine($"  {verb.Key} - {type.Type.FullName}");
                }
            }

            Console.WriteLine();
            Console.WriteLine("For help on a command, run: [COMMAND] --help");
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

        private void WriteSingleCommandNoArguments(Invocation invocation)
        {
            var binding = invocation.BindResults.First();

            var details = new AppInfo();
            var verbs = GetVerbs(binding);
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

        private string GetVerbs(CanBindResult binding)
        {
            return "";
        }

        public void WriteUnbound(Invocation invocation)
        {
            Console.WriteLine("Unknown command:");
            Console.WriteLine();
            Console.WriteLine($"    {invocation.Args}");
            Console.WriteLine();
            Console.WriteLine("Available commands:");
            Console.WriteLine();

            foreach (var bindResult in invocation.BindResults)
            {
                Console.WriteLine(bindResult.Type.Name); 
            }
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
