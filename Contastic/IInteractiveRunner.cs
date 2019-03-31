using System.Threading.Tasks;

namespace Contastic
{
    public interface IInteractiveRunner
    {
        Task<int> Run(string[] args);
    }
}