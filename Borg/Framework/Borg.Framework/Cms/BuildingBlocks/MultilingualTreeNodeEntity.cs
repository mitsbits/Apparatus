using Borg.Infrastructure.Core.DDD.Contracts;
using System;

namespace Borg.Framework.Cms.BuildingBlocks
{
    public abstract class MultilingualTreeNodeEntity<TKey, TLanguage> : MultilingualEntity<TKey, TLanguage> where TKey : IEquatable<TKey> where TLanguage : ILocalizationSilo
    {
        public virtual TKey ParentId { get; protected set; }
        public virtual int Depth { get; protected set; }
        public virtual string Hierarchy { get; protected set; }

        public virtual bool IsRoot() => ParentId == null || ParentId.Equals(default);
    }
}