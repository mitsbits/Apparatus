using Borg.Infrastructure.Core.DDD.Contracts;
using Borg.Infrastructure.Core.DDD.ValueObjects;
using System;

namespace Borg.Framework.Cms.BuildingBlocks
{
    public abstract class EntityCompositeKey<TKey> : EntityGeneric<TKey> where TKey : CompositeKey, IEquatable<TKey>
    {
    }

    public abstract class EntityGeneric<TKey> : IEntity<TKey> where TKey : IEquatable<TKey>
    {
        public virtual TKey Id { get; protected set; }

        public virtual CompositeKey Keys => CompositeKeyInternal();

        protected virtual CompositeKey CompositeKeyInternal()
        {
            return CompositeKeyBuilder.Create()
                .AddKey(nameof(Id)).AddValue(Id)
                .Build();
        }

        public override string ToString()
        {
            return $"{GetType().Name}:{Keys}";
        }
    }
}