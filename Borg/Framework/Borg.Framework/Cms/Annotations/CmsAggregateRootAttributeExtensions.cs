using Borg.Framework.Cms.Annotations;
using System;
using System.Collections.Concurrent;
using System.Reflection;

namespace Borg
{
    public static class CmsAggregateRootAttributeExtensions
    {
        private static readonly Lazy<ConcurrentDictionary<Type, string[]>> _cache
            = new Lazy<ConcurrentDictionary<Type, string[]>>(() => new ConcurrentDictionary<Type, string[]>());

        private static ConcurrentDictionary<Type, string[]> Cache => _cache.Value;

        public static string EntityPlural(this Type type)
        {
            return GetValues(type)[1];
        }

        public static string EntitySingular(this Type type)
        {
            return GetValues(type)[0];
        }

        private static string[] GetValues(Type type)
        {
            if (Cache.TryGetValue(type, out var values))
            {
                return values;
            }
            else
            {
                return Cache.GetOrAdd(type, PopulateValues(type));
            }
        }

        private static string[] PopulateValues(Type type)
        {
            var attr = type.GetCustomAttribute<CmsAggregateRootAttribute>();
            return (attr == null) ? new string[2] { string.Empty, string.Empty } : new string[2] { attr.Singular, attr.Plural };
        }
    }
}