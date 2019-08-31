using System;
using System.Threading;
using System.Threading.Tasks;

namespace Borg.Framework.DAL
{
    public interface IUnitOfWork : IQueryUnitOfWork, IReadWriteUnitOfWork, ITransactionalUnitOfWork, IDisposable
    {
    }

    public interface IQueryUnitOfWork
    {
        IQueryRepository<T> QueryRepo<T>() where T : class;
    }

    public interface IQuerableUnitOfWork
    {
        IQuerableRepository<T> QuerableRepo<T>() where T : class;
    }

    public interface IReadWriteUnitOfWork
    {
        IReadWriteRepository<T> ReadWriteRepo<T>() where T : class;
    }

    public interface ISearchUnitOfWork
    {
        ISearchRepository<T> Search<T>() where T : class;
    }

    public interface ITransactionalUnitOfWork
    {
        Task Save(CancellationToken cancelationToken = default, bool supreseEvents = false);
    }
}