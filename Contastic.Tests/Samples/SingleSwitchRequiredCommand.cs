﻿using System.Threading.Tasks;

namespace Contastic.Samples
{
    public class SingleSwitchRequiredCommand : ICommand
    {
        [Option(LongName = "single-switch", ShortName = 's', Required = true)]
        public bool Flag { get; set; }

        public Task<int> Execute()
        {
            throw new System.NotImplementedException();
        }
    }
}
