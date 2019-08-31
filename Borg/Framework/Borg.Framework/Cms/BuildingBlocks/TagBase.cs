using Borg.Infrastructure.Core.DDD.Contracts;
using System;

namespace Borg.Framework.Cms.BuildingBlocks
{
    public abstract class TagBase<TKey, TLanguage> : MultilingualEntity<TKey, TLanguage>, ITag where TKey : IEquatable<TKey> where TLanguage : IGlobalizationSilo
    {
        public virtual string Title { get; set; }
        public virtual string Slug { get; set; }
        public virtual bool IsActive { get; set; }
    }
}