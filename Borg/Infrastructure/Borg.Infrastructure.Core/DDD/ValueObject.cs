using Borg.Infrastructure.Core.DDD.ValueObjects;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Borg.Infrastructure.Core.DDD
{
    public abstract class ValueObject<T> : IEquatable<T> where T : ValueObject<T>
    {
        [ExcludeValueObjectField]
        private static Lazy<ConcurrentDictionary<Type, IEnumerable<PropertyInfo>>> _cache = new Lazy<ConcurrentDictionary<Type, IEnumerable<PropertyInfo>>>(() => new ConcurrentDictionary<Type, IEnumerable<PropertyInfo>>());

        private static ConcurrentDictionary<Type, IEnumerable<PropertyInfo>> Cache => _cache.Value;

        public virtual bool Equals(T other)

        {
            if (other == null)

                return false;

            var t = GetType();

            var otherType = other.GetType();

            if (t != otherType)

                return false;

            var fields = t.GetTypeInfo().DeclaredFields;

            foreach (var field in fields)

            {
                var value1 = field.GetValue(other);

                var value2 = field.GetValue(this);

                if (value1 == null)

                {
                    if (value2 != null)

                        return false;
                }
                else if (!value1.Equals(value2))

                {
                    return false;
                }
            }

            return true;
        }

        public override bool Equals(object obj)

        {
            if (obj == null)

                return false;

            var other = obj as T;

            return Equals(other);
        }

        public override int GetHashCode()

        {
            var fields = GetFields();

            var startValue = 17;

            var multiplier = 59;

            var hashCode = startValue;

            foreach (var field in fields)

            {
                var value = field.GetValue(this);

                if (value != null)

                    hashCode = hashCode * multiplier + value.GetHashCode();
            }

            return hashCode;
        }

        private IEnumerable<PropertyInfo> GetFieldsInternal()

        {
            var t = GetType();

            var fields = new List<PropertyInfo>();

            bool predicate(PropertyInfo x) => !x.IsExcludedFromValueObjectComparison();

            while (t != typeof(object))

            {
                fields.AddRange(t.GetTypeInfo().DeclaredProperties.Where(predicate));
                t = t.GetTypeInfo().BaseType;
            }

            return fields;
        }

        private IEnumerable<PropertyInfo> GetFields()
        {
            var t = GetType();

            if (Cache.TryGetValue(t, out IEnumerable<PropertyInfo> value))
            {
                return value;
            }
            var fields = GetFieldsInternal();
            Cache.TryAdd(t, fields);
            return fields;
        }

        public static bool operator ==(ValueObject<T> x, ValueObject<T> y)
        {
            return x.Equals(y);
        }

        public static bool operator !=(ValueObject<T> x, ValueObject<T> y)

        {
            return !(x == y);
        }
    }
}