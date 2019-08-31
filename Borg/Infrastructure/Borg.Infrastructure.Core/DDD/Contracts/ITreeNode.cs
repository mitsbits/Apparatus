using System;

namespace Borg.Infrastructure.Core.DDD.Contracts
{
    public interface ITreeNode<TKey> : ITreeNode, IEntity<TKey> where TKey : IEquatable<TKey>
    {
        TKey ParentId { get; }
    }

    public interface ITreeNode : IEntity
    {
        int Depth { get; }
        string Hierarchy { get; }
    }
}