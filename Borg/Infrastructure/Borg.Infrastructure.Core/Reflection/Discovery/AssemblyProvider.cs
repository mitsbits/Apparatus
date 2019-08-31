using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Borg.Infrastructure.Core.Reflection.Discovery
{
    public abstract class AssemblyProvider : IAssemblyProvider
    {
        protected ILogger Logger { get; }
        protected Func<Assembly, bool> Predicate { get; private set; }
        private static ConcurrentDictionary<Type, Assembly[]> cache = new ConcurrentDictionary<Type, Assembly[]>();

        protected AssemblyProvider(ILoggerFactory loggerFactory = null, Func<Assembly, bool> predicate = null)
        {
            Logger = loggerFactory != null ? loggerFactory.CreateLogger(GetType()) : NullLogger.Instance;
            Predicate = predicate;
        }

        public virtual IEnumerable<Assembly> GetAssemblies()
        {
            Assembly[] set;
            if (cache.TryGetValue(GetType(), out set)) return set;

            var source = new List<Assembly>();
            var candidates = Candidates();
            foreach (var candidate in candidates)
            {
                if (Predicate != null && !Predicate(candidate)) continue;
                if (source.Any(a => string.Equals(a.FullName, candidate.FullName, StringComparison.OrdinalIgnoreCase))) continue;
                source.Add(candidate);
            }
            set = source.ToArray();
            cache.AddOrUpdate(GetType(), set, (key, oldValue) => oldValue);
            return set;
        }

        protected abstract IEnumerable<Assembly> Candidates();
    }
}