using System;
using System.Threading;
using System.Threading.Tasks;

namespace Borg.Framework.Cache
{
    public interface ICacheClient
    {
        Task<T> Get<T>(string key, CancellationToken cancelationToken = default);

        Task Evict(string key, CancellationToken cancelationToken = default);

        Task Set<T>(string key, T value, TimeSpan? expiresIn = null, CancellationToken cancelationToken = default);
    }
}