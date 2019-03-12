using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Contastic.Binding;
using Contastic.Models;

namespace Contastic
{
    internal class HelpDescription
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

        private void WriteUnknownVerbs(Invocation invocation)
        {
            var details = AppDetails.Current(invocation);

            Console.WriteLine($"Unknown command: {invocation.Args}");
            Console.WriteLine();
            Console.WriteLine($@"Run ""dotnet .\{details.FileName} help"" for a list of commands.");
            
        }

        private void WriteNoArgumentsSpecificCommand(CanBindResult match)
        {
            var details = AppDetails.Current(null);

            Console.WriteLine(details.Description);
            Console.WriteLine();
            Console.WriteLine("USAGE");
            Console.WriteLine($"  $ dotnet .\\{details.FileName} {match.Verbs} {match.UnnamedArguments}");
            Console.WriteLine();


        }

        private void WriteMultipleCommandNoArguments(Invocation invocation)
        {
            var details = AppDetails.Current(invocation);

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

            foreach (var argument in binding.UnknownArguments)
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

            var details = AppDetails.Current(invocation);
            var verbs = GetVerbs(binding);
            var options = GetOptions(binding);

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

        private List<string> GetOptions(CanBindResult binding)
        {
            var options = new List<string>();

            var allOptions = new List<IOption>();
            allOptions.AddRange(binding.BoundArguments);
            allOptions.AddRange(binding.UnboundArguments);
            allOptions.AddRange(binding.BoundSwitches);
            allOptions.AddRange(binding.UnboundSwitches);

            allOptions = allOptions
                .OrderBy(o => o.ShortName)
                .ThenBy(o => o.LongName)
                .ToList();

            foreach (var option in allOptions)
            {
                var optionString = $"  ";

                if (option.ShortName != '\0')
                {
                    optionString += $"-{option.ShortName}";
                }

                if (string.IsNullOrEmpty(option.LongName) == false)
                {
                    if (string.IsNullOrWhiteSpace(optionString) == false)
                    {
                        optionString += ", ";
                    }

                    optionString += $"--{option.LongName}";
                }

                // TODO: Property Name / Description

                if (string.IsNullOrWhiteSpace(optionString) == false)
                {
                    options.Add(optionString);
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

        private class AppDetails
        {
            public string FileName { get; set; }

            public string Prompt { get; set; }

            public string Description { get; set; }

            public static AppDetails Current(Invocation invocation)
            {
                var details = new AppDetails();

                var assembly = Assembly.GetEntryAssembly();
                var processName = assembly.Location;
                details.FileName = Path.GetFileName(processName);
                details.Description = GetCommandDescription();
                details.Prompt = ">";

                return details;
            }        
            
            private static string GetCommandDescription()
            {
                var sb = new StringBuilder();

                var assembly = Assembly.GetEntryAssembly();

                var title = assembly
                    .GetCustomAttributes(typeof(AssemblyTitleAttribute), true)
                    .Cast<AssemblyTitleAttribute>()
                    .FirstOrDefault();

                if (title != null)
                {
                    if (!string.IsNullOrWhiteSpace(title.Title))
                    {
                        sb.AppendLine(title.Title);
                    }
                }

                var descriptionAttribute = assembly
                    .GetCustomAttributes(typeof(AssemblyDescriptionAttribute), true)
                    .Cast<AssemblyDescriptionAttribute>()
                    .FirstOrDefault();

                if (descriptionAttribute != null)
                {
                    if (!string.IsNullOrWhiteSpace(descriptionAttribute.Description))
                    {
                        sb.AppendLine(descriptionAttribute.Description);
                    }
                }

                return sb.ToString();
            }
        }
    }
}
