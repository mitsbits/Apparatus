using Borg.Framework.Storage.Assets;
using Borg.Framework.Storage.Assets.Contracts;
using Borg.Infrastructure.Core.DDD.Contracts;
using Borg.Infrastructure.Core.DDD.ValueObjects;
using System;

namespace Borg.Framework.Cms.BuildingBlocks
{
    public abstract class DocumentBase<TKey> : AssetInfoDefinition<TKey>, IHaveVersion, IEntity<TKey> where TKey : IEquatable<TKey>
    {
        protected DocumentBase() : base(default(TKey), string.Empty, DocumentBehaviourState.Processing)
        {
        }

        protected DocumentBase(TKey id, string name, DocumentBehaviourState state = DocumentBehaviourState.Commited) : base(id, name, state)
        {
        }

        public override CompositeKey Keys => CompositeKeyInternal();

        public virtual int Version => CurrentFile.Version;

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