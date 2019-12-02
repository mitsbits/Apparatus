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
        private static Lazy<ConcurrentDictionary<Type, IEnumerable<FieldInfo>>> _cache = new Lazy<ConcurrentDictionary<Type, IEnumerable<FieldInfo>>>(() => new ConcurrentDictionary<Type, IEnumerable<FieldInfo>>());
        
        private static ConcurrentDictionary<Type, IEnumerable<FieldInfo>> Cache => _cache.Value;

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

        private IEnumerable<FieldInfo> GetFieldsInternal()

        {
            var t = GetType();

            var fields = new List<FieldInfo>();

            while (t != typeof(object))

            {
                //TODO: create a HasAttribute extension
                fields.AddRange(t.GetTypeInfo().DeclaredFields.Where(x=> x.GetCustomAttribute<ExcludeValueObjectFieldAttribute>() == null)); 
                t = t.GetTypeInfo().BaseType;
            }

            return fields;
        }

        private IEnumerable<FieldInfo> GetFields()
        {
            var t = GetType();

            if (Cache.TryGetValue(t, out IEnumerable<FieldInfo> value))
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