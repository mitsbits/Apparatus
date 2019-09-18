using Borg.Framework.DAL;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Borg.Framework.Azure.Storage.Tables
{
    public interface IAzureTableStoreRepository<T> : IRepository<T> where T : IHaveTableKey
    {
        Task<T> Create(T entity, CancellationToken cancellationToken = default(CancellationToken));

        Task<T> Get(AzureTableCompositeKey key, CancellationToken cancellationToken = default(CancellationToken));

        Task<IEnumerable<T>> Find(string predicate, CancellationToken cancellationToken = default(CancellationToken));

        Task Delete(AzureTableCompositeKey key, CancellationToken cancellationToken = default(CancellationToken));
    }
}