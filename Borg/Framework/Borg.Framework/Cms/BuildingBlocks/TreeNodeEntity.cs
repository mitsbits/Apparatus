using Borg.Infrastructure.Core.DDD.Contracts;
using System;

namespace Borg.Framework.Cms.BuildingBlocks
{
    public abstract class TreeNodeEntity<TKey> : Entity<TKey> where TKey : IEquatable<TKey>
    {
        public TKey ParentId { get; protected set; }

        public int Depth { get; protected set; }

        public string Hierarchy { get; protected set; }
    }
}