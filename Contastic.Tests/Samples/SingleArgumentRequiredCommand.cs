using System.Threading.Tasks;

namespace Contastic.Samples
{
    public class SingleArgumentRequiredCommand : ICommand
    {
        [Parameter(LongName = "single-argument", ShortName = 's', Required = true)]
        public string Argument { get; set; }

        public Task<int> Execute()
        {
            throw new System.NotImplementedException();
        }
    }
}
