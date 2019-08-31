using System;

namespace Borg.Infrastructure.Core.DDD.Contracts
{
    public interface IMultilingual<TKey> : IMultilingual where TKey : IEquatable<TKey>
    {
        TKey LanguageId { get; set; }
    }

    public interface IMultilingual
    {
    }
}