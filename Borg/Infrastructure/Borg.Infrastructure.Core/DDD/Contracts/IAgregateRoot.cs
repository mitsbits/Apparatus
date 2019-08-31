using System;

namespace Borg.Infrastructure.Core.DDD.Contracts
{
    public interface IAgregateRoot<out TKey> : IAgregateRoot, IEntity<TKey> where TKey : IEquatable<TKey>
    {
    }

    public interface IAgregateRoot
    {
        int Version { get; }
    }
}