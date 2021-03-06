﻿using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Contastic.Commands
{
    [Verb("copy")]
    [Description("Copy data from the source loation to destination.")]
    public class CopyCommand : ICommand
    {
        [Description("Source location")]
        [Argument(Order = 0, Required = true)]
        public string Source { get; set; }

        [Description("Destination location")]
        [Argument(Order = 1, Required = true)]
        public string Destination { get; set; }

        [Description("The username to use to connect")]
        [Option(ShortName = 'u', LongName = "username")]
        public string UserName { get; set; }

        [Description("The password to use to connect")]
        [Option(ShortName = 'p', LongName = "password")]
        public string Password { get; set; }

        public Task<int> Execute()
        {
            Console.WriteLine("Copy:");
            Console.WriteLine($"  Source      : {Source}");
            Console.WriteLine($"  Destination : {Destination}");
            Console.WriteLine($"  User Name   : {UserName}");
            Console.WriteLine($"  Password    : {Password}");

            return Task.FromResult(0);
        }
    }
}
