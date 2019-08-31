using Borg.Infrastructure.Core.DDD.ValueObjects;

namespace Borg.Infrastructure.Core.DDD.Contracts
{
    public interface IIdentifiable
    {
        CompositeKey Keys { get; }
    }
}