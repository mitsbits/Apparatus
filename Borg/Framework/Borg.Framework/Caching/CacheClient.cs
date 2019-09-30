using Borg.Infrastructure.Core;
using Borg.Infrastructure.Core.Services.Serializer;
using Borg.Infrastructure.Core.Threading;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Borg.Framework.Caching
{
    public class CacheClient : ICacheClient
    {
        private readonly ILogger logger;
        private readonly ISerializer serializer;
        private readonly IDistributedCache cache;
        private static readonly Lazy<AsyncLock> asyncLock = new Lazy<AsyncLock>(() => new AsyncLock());
        private AsyncLock AsyncLock => asyncLock.Value;

        public CacheClient(ISerializer serializer, ILoggerFactory loggerFactory = null)
        {
            this.logger = loggerFactory == null ? NullLogger.Instance : loggerFactory.CreateLogger(GetType());
            this.serializer = Preconditions.NotNull(serializer, nameof(serializer));
            this.cache = Preconditions.NotNull(cache, nameof(cache));
        }

        public async Task Evict(string key, CancellationToken cancelationToken = default)
        {
            cancelationToken.ThrowIfCancellationRequested();
            await cache.RemoveAsync(key, cancelationToken);
            logger.Trace("Evicted entry {key}", key);
        }

        public async Task<T> Get<T>(string key, CancellationToken cancelationToken = default)
        {
            cancelationToken.ThrowIfCancellationRequested();
            var bytes = await cache.GetAsync(key, cancelationToken);
            if (bytes == null)
            {
                logger.Trace("Cache miss for key {key}", key);
                return default(T);
            }
            return await serializer.DeserializeAsync<T>(bytes); //TODO: cancelationToken here
        }

        public async Task Set(string key, object value, CacheEntryOptions options = null, CancellationToken cancelationToken = default)
        {
            cancelationToken.ThrowIfCancellationRequested();
            var data = await serializer.Serialize(value);
            if (data != null)
            {
                var opts = MapOptions(options);
                using (AsyncLock.Lock())
                {
                    if (opts == null)
                    {
                        await cache.SetAsync(key, data, cancelationToken);
                    }
                    else
                    {
                        await cache.SetAsync(key, data, opts, cancelationToken);
                    }
                }
            }
        }

        private DistributedCacheEntryOptions MapOptions(CacheEntryOptions source)
        {
            if (source == null) return null;

            var result = new DistributedCacheEntryOptions();
            if (source.Expires.HasValue)
            {
                result.SetAbsoluteExpiration(source.Expires.Value);
            }

            return result;
        }
    }
}