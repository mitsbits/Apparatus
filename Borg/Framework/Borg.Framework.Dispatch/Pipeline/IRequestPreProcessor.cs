using System.Threading;
using System.Threading.Tasks;

namespace Borg.Framework.Dispatch.Pipeline
{
    public interface IRequestPreProcessor<in TRequest>
    {
        Task Process(TRequest request, CancellationToken cancellationToken);
    }
}