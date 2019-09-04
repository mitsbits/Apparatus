using System.Collections.Generic;

namespace Borg.Infrastructure.Core.DDD.Contracts
{
    public interface IHaveIdentity
    {
        IDictionary<string, object> Identity { get; }
    }
}