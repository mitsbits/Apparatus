using Borg.Infrastructure.Core.DDD.ValueObjects;
using System.Threading;
using System.Threading.Tasks;

namespace Borg.Framework.Dispatch.Contracts
{
    public interface IRequestHandler<in TRequest, TResponse>
    {
        Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
    }

    public interface IRequestHandler<in TRequest> : IRequestHandler<TRequest, Unit>
    {
    }

    public abstract class AsyncRequestHandler
    {
        public abstract Task<object> Handle(object request, CancellationToken cancellationToken);
    }

    public abstract class AsyncRequestHandler<TRequest> : AsyncRequestHandler, IRequestHandler<TRequest>
    {
        async Task<Unit> IRequestHandler<TRequest, Unit>.Handle(TRequest request, CancellationToken cancellationToken)
        {
            await Handle(request, cancellationToken);
            return Unit.Value;
        }
    }

    public abstract class AsyncRequestHandler<TRequest, TResponse> : AsyncRequestHandler, IRequestHandler<TRequest, TResponse>
    {
        async Task<TResponse> IRequestHandler<TRequest, TResponse>.Handle(TRequest request, CancellationToken cancellationToken)
        {
            var result = await Handle(request, cancellationToken);
            return (TResponse)result;
        }
    }

    public abstract class RequestHandler
    {
        public abstract object Handle(object request);
    }

    public abstract class RequestHandler<TRequest> : RequestHandler, IRequestHandler<TRequest>
    {
        Task<Unit> IRequestHandler<TRequest, Unit>.Handle(TRequest request, CancellationToken cancellationToken)
        {
            Handle(request);
            return Unit.Task;
        }
    }

    public abstract class RequestHandler<TRequest, TResponse> : RequestHandler, IRequestHandler<TRequest, TResponse>
    {
        public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
        {
            var result = Handle(request);
            return Task.FromResult((TResponse)result);
        }
    }
}