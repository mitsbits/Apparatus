using Borg.Infrastructure.Core.DDD;

namespace Borg.Framework.Cache
{
    public class CacheValue<T> : ValueObject<CacheValue<T>>
    {
        public CacheValue(T value, bool hasValue)
        {
            Value = value;
            HasValue = hasValue;
        }

        public bool HasValue { get; }

        public bool IsNull => Value == null;

        public T Value { get; }

        public static CacheValue<T> Null => new CacheValue<T>(default, true);

        public static CacheValue<T> NoValue => new CacheValue<T>(default, false);

        public override string ToString()
        {
            return Value?.ToString() ?? "<null>";
        }
    }
}