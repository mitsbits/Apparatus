using Borg.Framework.Caching;
using Borg.Infrastructure.Core;
using System.Threading;
using System.Threading.Tasks;

namespace Borg
{
    public static class ICacheClientExtensions
    {
        public static async Task<T> Get<T>(this ICacheClient cache, string key, CancellationToken cancelationToken = default)
        {
            cancelationToken.ThrowIfCancellationRequested();
            cache = Preconditions.NotNull(cache, nameof(cache));
            var value = await cache.Get(key, cancelationToken);
            return (T)value;
        }
    }
}