using Borg.Infrastructure.Core;
using Borg.Infrastructure.Core.Services.Serializer;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Borg.Framework.Cache
{
    public class CacheClient : ICacheClient
    {
        private readonly IDistributedCache cache;
        private readonly ISerializer serializer;
        private readonly ILogger logger;
        private readonly TimeSpan aLongTime = TimeSpan.FromDays(365 * 10);

        public CacheClient(ILoggerFactory loggerFactory, IDistributedCache cache, ISerializer serializer)
        {
            this.cache = Preconditions.NotNull(cache, nameof(cache));
            this.serializer = Preconditions.NotNull(serializer, nameof(serializer));
            logger = loggerFactory == null ? NullLogger.Instance : loggerFactory.CreateLogger(GetType());
        }

        public async Task Evict(string key, CancellationToken cancelationToken = default)
        {
            cancelationToken.ThrowIfCancellationRequested();
            await cache.RemoveAsync(key, cancelationToken);
        }

        public async Task<T> Get<T>(string key, CancellationToken cancelationToken = default)
        {
            cancelationToken.ThrowIfCancellationRequested();
            key = Preconditions.NotEmpty(key, nameof(key));
            var bytes = await cache.GetAsync(key, cancelationToken);
            if (bytes == null || !bytes.Any()) return default;
            if (typeof(T).Equals(typeof(string)))
            {
                var str = await serializer.Deserialize(bytes, typeof(string));
                return (T)str;
            }

            var hit = await serializer.DeserializeAsync<T>(bytes);
            serializer.SerializeToString(bytes);
            return hit;
        }

        public async Task Set<T>(string key, T value, TimeSpan? expiresIn = null, CancellationToken cancelationToken = default)
        {
            cancelationToken.ThrowIfCancellationRequested();
            key = Preconditions.NotEmpty(key, nameof(key));
            if (value == null) throw new ArgumentNullException(nameof(value));
            var bytes = await serializer.Serialize(value);
            if (expiresIn == null) expiresIn = aLongTime;
            var options = new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = expiresIn
            };
            await cache.SetAsync(key, bytes, options, cancelationToken);
        }
    }
}