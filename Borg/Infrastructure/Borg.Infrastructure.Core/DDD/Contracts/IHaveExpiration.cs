using System;

namespace Borg.Infrastructure.Core.DDD.Contracts
{
    public interface IHaveExpiration
    {
        DateTimeOffset? Expiration { get; set; }
    }
}