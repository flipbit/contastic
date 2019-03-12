using System.Threading.Tasks;

namespace Contastic.Samples
{
    [Verb("single-class-verb")]
    public class SingleVerbCommand : ICommand
    {
        public Task<int> Execute()
        {
            throw new System.NotImplementedException();
        }
    }
}
