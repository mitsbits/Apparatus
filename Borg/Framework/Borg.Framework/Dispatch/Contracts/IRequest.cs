using Borg.Infrastructure.Core.DDD.ValueObjects;

namespace Borg.Framework.Dispatch.Contracts
{
    public interface IRequest : IRequest<Unit> { }

    public interface IRequest<out TResponse> : IBaseRequest { }

    public interface IBaseRequest { }
}