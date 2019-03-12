using System.Threading.Tasks;

namespace Contastic.Samples
{
    [Verbs("copy", "from", "to")]
    public class SingleVerbsCommand : ICommand
    {
        public Task<int> Execute()
        {
            throw new System.NotImplementedException();
        }
    }
}
