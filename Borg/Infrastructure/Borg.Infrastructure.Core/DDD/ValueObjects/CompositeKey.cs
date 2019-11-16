using Borg.Infrastructure.Core.Reflection.Discovery.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static Borg.Infrastructure.Core.DDD.ValueObjects.CompositeKeyBuilder;

namespace Borg.Infrastructure.Core.DDD.ValueObjects
{
    [Serializable]
    [MapperIgnore]
    public class CompositeKey : ValueObject<CompositeKey>, IReadOnlyDictionary<string, object>, IEquatable<CompositeKey>
    {
        protected readonly Dictionary<string, object> _data;

        public IEnumerable<string> Keys => _data.Keys;

        public IEnumerable<object> Values => _data.Values;

        public int Count => _data.Count();

        public object this[string key] => _data[key];

        public CompositeKey(string queryString) : this()
        {
            queryString = Preconditions.NotEmpty(queryString.TrimStart('?', '&'), nameof(queryString));
            var parts = queryString.Split('&');
            foreach (var part in parts)
            {
                var localparts = part.Split('=');
                Add(localparts[0], localparts[1]);
            }
        }

        public CompositeKey(IEnumerable<KeyValuePair<string, object>> source) : this(source.Select(x => (key: x.Key, value: x.Value)))
        {
        }

        public CompositeKey(IEnumerable<(string key, object value)> source) : this()
        {
            source = Preconditions.NotEmpty(source, nameof(source));
            foreach (var incoming in source)
            {
                Add(incoming.key, incoming.value);
            }
        }

        public CompositeKey()
        {
            var comparer = StringComparer.InvariantCultureIgnoreCase;
            _data = new Dictionary<string, object>(comparer);
        }

        public virtual void Add(string key, object value)
        {
            if (_data.ContainsKey(key))
            {
                _data[key] = value;
            }
            else
            {
                _data.Add(key, value);
            }
        }

        public override string ToString()
        {
            return string.Join("", _data.Select(x => $"[{x.Key}:{x.Value}]"));
        }

        public bool ContainsKey(string key)
        {
            return _data.ContainsKey(key);
        }

        public bool TryGetValue(string key, out object value)
        {
            return _data.TryGetValue(key, out value);
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public string ToQueryString()
        {
            return string.Join("&", _data.Select(x => $"{x.Key}={x.Value}"));
        }
    }

    public class CompositeKeyBuilder : ICanAddKey, ICanAddValue, ICanBuildCompositeKey, ICanAddKeyOrBuild
    {
        private readonly IList<(string key, object value)> source;

        private CompositeKeyBuilder()
        {
            source = new List<(string key, object value)>();
        }

        public static ICanAddKey Create()
        {
            return new CompositeKeyBuilder();
        }

        public ICanAddValue AddKey(string key)
        {
            key = Preconditions.NotEmpty(key, nameof(key));
            if (source.Any(x => string.Equals(x.key, key, StringComparison.InvariantCultureIgnoreCase))) throw new ApplicationException($"{nameof(key)} already in {nameof(source)}");
            var tuple = (key: key, value: default(object));
            source.Add(tuple);
            return this as ICanAddValue;
        }

        public ICanAddKeyOrBuild AddValue(object value)
        {
            value = Preconditions.NotNull(value, nameof(value));
            var last = source.Last();
            var tuple = (key: last.key, value: value);
            source.RemoveAt(source.Count() - 1);
            source.Add(tuple);
            return this as ICanAddKeyOrBuild;
        }

        public CompositeKey Build()
        {
            return new CompositeKey(source);
        }

        public interface ICanAddKey
        {
            ICanAddValue AddKey(string key);
        }

        public interface ICanAddKeyOrBuild : ICanAddKey, ICanBuildCompositeKey
        {
        }

        public interface ICanAddValue
        {
            ICanAddKeyOrBuild AddValue(object value);
        }

        public interface ICanBuildCompositeKey
        {
            CompositeKey Build();
        }
    }
}