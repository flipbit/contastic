using System;

namespace Contastic.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            var runner = new DemoRunner();
            runner.Initialize(typeof(Program).Assembly);

            while (true)
            {
                Console.Write(">");

                var command = Console.ReadLine();

                var returnCode = runner.Run(command);

                if (returnCode == -1)
                {
                    Console.WriteLine("Unknown command: {0}", command);
                }
            }
        }
    }
}
