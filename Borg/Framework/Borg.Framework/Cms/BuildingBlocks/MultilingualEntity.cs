using Borg.Infrastructure.Core.DDD.Contracts;
using Borg.Infrastructure.Core.DDD.ValueObjects;
using System;

namespace Borg.Framework.Cms.BuildingBlocks
{
    public abstract class MultilingualEntity<TKey, TLanguage> : IMultilingualEntity<TKey, TLanguage> where TKey : IEquatable<TKey> where TLanguage : ILocalizationSilo
    {
        public TKey Id { get; protected set; }

        public virtual CompositeKey Keys => CompositeKeyInternal();

        public virtual TLanguage Language { get; protected set; }

        public TKey LanguageID { get; protected set; }

        protected virtual CompositeKey CompositeKeyInternal()
        {
            return CompositeKeyBuilder.Create()
                .AddKey(nameof(Id)).AddValue(Id)
                .AddKey(nameof(LanguageID)).AddValue(LanguageID)
                .Build();
        }

        public override string ToString()
        {
            return $"{GetType().Name}:{Keys}";
        }
    }
}