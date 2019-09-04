using System.Threading;
using System.Threading.Tasks;

namespace Borg.Framework.Dispatch.Contracts
{
    public interface IDispatcher : IDispatcheBlockingSender, IDispatchePublishSender
    {
    }

    public interface IDispatcheBlockingSender
    {
        Task Send(object request, CancellationToken cancellationToken = default);

        Task<TResponse> Send<TResponse, TRequest>(TRequest request, CancellationToken cancellationToken = default);
    }

    public interface IDispatchePublishSender
    {
        Task Publish(object notification, CancellationToken cancellationToken = default);

        Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default);
    }
}