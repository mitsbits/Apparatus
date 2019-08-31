using Borg.Infrastructure.Core.DI;
using System;
using System.Collections.Concurrent;

namespace Borg
{
    public static class PlugableServiceAttributeExtensions
    {
        private static ConcurrentDictionary<Type, bool> cache = new ConcurrentDictionary<Type, bool>();

        public static bool IsPlugableService(this Type type)
        {
            if (type == null || type.IsInterface || type.IsAbstract) return false;
            if (cache.TryGetValue(type, out var result)) return result;
            result = type.HasAttribute<PlugableServiceAttribute>();
            cache.TryAdd(type, result);
            return result;
        }
    }
}