using Borg.Infrastructure.Core.DDD.Contracts;
using Borg.Infrastructure.Core.DDD.ValueObjects;
using System;

namespace Borg.Framework.Cms.BuildingBlocks
{
    public abstract class TreeNodeEntity<TKey> : Entity<TKey> where TKey : IEquatable<TKey>
    {
        public TKey ParentId { get; protected set; }

        public int Depth { get; protected set; }

        public string Hierarchy { get; protected set; }
    }

    public abstract class Entity<TKey> : IEntity<TKey> where TKey : IEquatable<TKey>
    {


        public CompositeKey Keys => throw new NotImplementedException();

        public TKey Id => throw new NotImplementedException();


    }
}