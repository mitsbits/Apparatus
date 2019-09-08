using System.Threading;
using System.Threading.Tasks;

namespace Borg.Framework.Caching
{
    public interface ICacheClient
    {
        Task<object> Get(string key, CancellationToken cancelationToken = default);

        Task Evict(string key, CancellationToken cancelationToken = default);

        Task Set(string key, object value, CacheEntryOptions options = default, CancellationToken cancelationToken = default);
    }
}