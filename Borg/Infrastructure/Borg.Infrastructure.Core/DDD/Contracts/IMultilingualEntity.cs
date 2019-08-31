using System;

namespace Borg.Infrastructure.Core.DDD.Contracts
{
    public interface IMultilingualEntity<TKey, out TLanguage> : IEntity<TKey>, IHaveLanguage<TKey, TLanguage> where TKey : IEquatable<TKey> where TLanguage : IGlobalizationSilo
    {
    }
}