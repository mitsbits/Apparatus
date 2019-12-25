using System;

namespace Borg.Infrastructure.Core.DDD.Contracts
{
    public interface IHaveWeight
    {
        double Weight { get; }
    }

    public interface IHaveWidth
    {
        int Width { get; }
    }

    public interface IHaveHeight
    {
        int Height { get; }
    }

    public interface IHaveKey<out TKey> where TKey : IEquatable<TKey>
    {
        TKey Key { get; }
    }

    public interface IHaveValue<out TVal>
    {
        TVal Value { get; }
    }

    public interface IKeyValuePair<out TKey, out TVal> : IHaveKey<TKey>, IHaveValue<TVal> where TKey : IEquatable<TKey>
    {
    }
}