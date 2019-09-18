using Borg.Framework.DAL;
using Microsoft.WindowsAzure.Storage.Table;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Borg.Framework.Azure.Storage.Tables
{
    public interface IAzureTableRepository<T> : IRepository<T> where T : TableEntity
    {
        Task<T> Create(T entity, CancellationToken cancellationToken = default(CancellationToken));

        Task<T> Get(AzureTableCompositeKey key, CancellationToken cancellationToken = default(CancellationToken));

        Task<IEnumerable<T>> Find(string predicate, CancellationToken cancellationToken = default(CancellationToken));

        Task Delete(AzureTableCompositeKey key, CancellationToken cancellationToken = default(CancellationToken));
    }
}