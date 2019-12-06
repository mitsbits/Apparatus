using System;

namespace Borg.Infrastructure.Core.DDD.Contracts
{
    public interface IMultilingualTreeNode<TKey, out TLanguage> : IEntity<TKey>, ITreeNode<TKey>, IHaveLanguage<TKey, TLanguage> where TKey : IEquatable<TKey> where TLanguage : ILocalizationSilo
    {
    }
}