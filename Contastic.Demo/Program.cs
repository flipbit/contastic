using System;
using System.Diagnostics;

namespace Contastic.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            var runner = new CommandRunner();
            
            runner.Initialize(typeof(Program).Assembly);

            var exitCode = runner.Run(args);

            if (exitCode == -1)
            {
                Console.WriteLine("Usage: contastic.demo.exe [args]");
                Console.WriteLine("");
                Console.WriteLine("Where [args]:");
                Console.WriteLine("");
                Console.WriteLine("-echo [message]    Print a message to the screen");
                Console.WriteLine("-time              Print the time to the screen");
            }
#if DEBUG
            if (Debugger.IsAttached)
            {
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
#endif

            Environment.Exit(exitCode);
        }
    }
}
