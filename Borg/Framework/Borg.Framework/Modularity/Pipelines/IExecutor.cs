using System.Threading;
using System.Threading.Tasks;

namespace Borg.Framework.Modularity.Pipelines
{
    public interface IExecutor
    {
        Task Execute(CancellationToken cancelationToken);
    }
}
