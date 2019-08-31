using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Linq;

namespace Borg.Infrastructure.Core.Reflection.Discovery.ObjectGraph
{
    public class ComplexTypeRecursor : IDisposable
    {
        private readonly ComplexTypeRecursorResult source;
        private readonly ILogger logger;
        private int recursionLevel;
        private Type currentReferer;
        private ComplexTypeRecursorConfiguration configuration = new ComplexTypeRecursorConfiguration();

        public ComplexTypeRecursor(Type root, Action<ComplexTypeRecursorConfiguration> config = null, ILogger logger = null)
        {
            source = new ComplexTypeRecursorResult();
            this.logger = logger ?? NullLogger.Instance;
            if (config != null) config.Invoke(configuration);

            recursionLevel = 0;

            DiscoverComplexTypes(root);
        }

        public ComplexTypeRecursorResult Results()
        {
            return Preconditions.NotNull(source, nameof(source));
        }

        private void DiscoverComplexTypes(Type root)
        {
            logger.Trace($"{nameof(ComplexTypeRecursor)} discovering {root.FullName} | Recursion {recursionLevel}");
            source.TryAdd(root, currentReferer, recursionLevel);
            recursionLevel++;
            currentReferer = root;
            foreach (var prop in root.GetProperties())
            {
                if (configuration.ExcludeSimples && prop.IsSimple()) continue;
                if (configuration.ExcludeTuples && prop.IsValueTupleType()) continue;
                if (configuration.ExcludeByAttribute && prop.IsMapperIgnore()) continue;
                Type target = null;
                if (prop.IsEnumerator())
                {
                    var collectionType = prop.GetGenericArgumentType();
                    if (configuration.ExcludeSimples && collectionType.IsSimple()) continue;
                    if (configuration.ExcludeTuples && collectionType.IsValueTupleType()) continue;
                    if (configuration.ExcludeByAttribute && collectionType.IsMapperIgnore()) continue;
                    target = prop.GetGenericArgumentType();
                }
                else
                {
                    target = prop.PropertyType;
                }
                if (target != null && !source.Any(x => target.Equals(x.Type)))
                {
                    DiscoverComplexTypes(target);
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
            }
        }
    }
}