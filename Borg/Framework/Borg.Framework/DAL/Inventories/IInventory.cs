using Borg.Framework.DAL.Ordering;
using Borg.Infrastructure.Core.Collections;
using Borg.Infrastructure.Core.DDD.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Borg.Framework.DAL.Inventories
{
    public interface IInventory
    {
        bool SupportsReads { get; }
        bool SupportsWrites { get; }
        bool SupportsQueries { get; }
        bool SupportsQuerable { get; }
        bool SupportsProjections { get; }
        bool SupportsSearch { get; }
        bool SupportsFactory { get; }
    }

    public interface IInventoryFacade<T> : IInventory<T>, IReadInventory<T>, IWriteInventory<T>, IQueryInventory<T>, IFactoryInventory<T> where T : class
    {
    }

    public interface IInventory<T> : IInventory
    {
    }

    public interface IReadInventory<T> : IInventory<T> where T : class
    {
        Task<IPagedResult<T>> Read(Expression<Func<T, bool>> predicate, int page, int size,
                IEnumerable<OrderByInfo<T>> orderByy, CancellationToken cancellationToken = default);

        Task<T> Get(CompositeKey compositeKey, CancellationToken cancellationToken = default);
    }

    public interface IWriteInventory<T> : IInventory<T> where T : class
    {
        Task<T> Create(T entity, CancellationToken cancellationToken = default);

        Task<T> Update(T entity, CancellationToken cancellationToken = default);

        Task Delete(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

        Task Delete(T entity, CancellationToken cancellationToken = default);
    }

    public interface IQuerableInventory<T> : IInventory<T> where T : class
    {
        IQueryable<T> Query { get; }
    }

    public interface IQueryInventory<T> : IInventory<T> where T : class
    {
        Task<IPagedResult<T>> Find(Expression<Func<T, bool>> predicate, int page, int records,
                IEnumerable<OrderByInfo<T>> orderBy = null, CancellationToken cancellationToken = default);
    }

    public interface ISearchInventory<T> : IInventory<T> where T : class
    {
        Task<IPagedResult<T>> Find(string term, int page, int size,
            IEnumerable<OrderByInfo<T>> orderBy, CancellationToken cancellationToken = default);
    }

    public interface IFactoryInventory<T> : IInventory<T> where T : class
    {
        Task<T> Instance();
    }

    public interface IProjectionInventory<T, TProjection>
    {
    }
}