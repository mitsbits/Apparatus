using Borg.Framework.DAL;
using Borg.Framework.DAL.Inventories;
using Borg.Framework.DAL.Ordering;
using Borg.Framework.EF.Contracts;
using Borg.Infrastructure.Core;
using Borg.Infrastructure.Core.Collections;
using Borg.Infrastructure.Core.DDD.ValueObjects;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Borg.Framework.EF.DAL.Inventories
{
    public abstract class InventoryBase<T, TDbContext> : IInventory<T> where T : class where TDbContext : BorgDbContext
    {
        protected readonly ILogger Logger;
        protected readonly IUnitOfWork<TDbContext> UOW;

        protected InventoryBase(ILoggerFactory loggerFactory, IUnitOfWork<TDbContext> unitOfWork)
        {
            Logger = loggerFactory != null ? loggerFactory.CreateLogger(GetType()) : NullLogger.Instance;
            UOW = Preconditions.NotNull(unitOfWork, nameof(unitOfWork));
        }

        public bool SupportsReads { get { return UOW.GetType().IsAssignableFrom(typeof(IReadWriteUnitOfWork)); } }
        public bool SupportsWrites { get { return UOW.GetType().IsAssignableFrom(typeof(IReadWriteUnitOfWork)); } }
        public bool SupportsQuerable { get { return UOW.GetType().IsAssignableFrom(typeof(IQuerableUnitOfWork)); } }
        public bool SupportsQueries { get { return UOW.GetType().IsAssignableFrom(typeof(IQueryUnitOfWork)); } }
        public bool SupportsProjections { get; protected set; } = false;
        public bool SupportsSearch { get { return UOW.GetType().IsAssignableFrom(typeof(ISearchUnitOfWork)); } }

        public bool SupportsFactory => true;
    }

    public class Inventory<T, TDbContext> : InventoryBase<T, TDbContext>, IInventoryFacade<T> where T : class where TDbContext : BorgDbContext
    {
        public Inventory(ILoggerFactory loggerFactory, IUnitOfWork<TDbContext> unitOfWork) : base(loggerFactory, unitOfWork)
        {
        }

        private IReadWriteRepository<T> ReadWrite => UOW.ReadWriteRepo<T>();
        private IQueryRepository<T> Query => UOW.QueryRepo<T>();

        public Task<T> Create(T entity, CancellationToken cancellationToken = default)
        {
            return ReadWrite.Create(entity, cancellationToken);
        }

        public Task Delete(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return ReadWrite.Delete(predicate, cancellationToken);
        }

        public Task Delete(T entity, CancellationToken cancellationToken = default)
        {
            return ReadWrite.Delete(entity, cancellationToken);
        }

        public Task<IPagedResult<T>> Read(Expression<Func<T, bool>> predicate, int page, int size, IEnumerable<OrderByInfo<T>> orderBy, CancellationToken cancellationToken = default)
        {
            return ReadWrite.Read(predicate, page, size, orderBy, cancellationToken);
        }

        public Task<T> Instance()
        {
            return UOW.Instance<T>();
        }

        public Task<T> Update(T entity, CancellationToken cancellationToken = default)
        {
            return ReadWrite.Update(entity, cancellationToken);
        }

        public Task<IPagedResult<T>> Find(Expression<Func<T, bool>> predicate, int page, int records, IEnumerable<OrderByInfo<T>> orderBy = null, CancellationToken cancellationToken = default)
        {
            return Query.Find(predicate, page, records, orderBy, cancellationToken);
        }

        public async Task<T> Get(CompositeKey compositeKey, CancellationToken cancellationToken = default)
        {
            var exprBuilder = new StringBuilder("x=> ");

            foreach (var prop in typeof(T).GetProperties())
            {
                if (compositeKey.TryGetValue(prop.Name, out var value))
                {
                    if (prop.PropertyType.Equals(typeof(string)))
                    {
                        exprBuilder.Append($"x.{prop.Name} == ").Append($"\"{value}\"");
                    }
                    else if (prop.PropertyType.Equals(typeof(bool)))
                    {
                        Func<int> localValue = () => (bool)value ? 1 : 0;

                        exprBuilder.Append($"x.{prop.Name} == {localValue.Invoke()}");
                    }
                    else
                    {
                        exprBuilder.Append($"x.{prop.Name} == {value}");
                    }
                    exprBuilder.Append(" && ");
                }

            }

            var exp = exprBuilder.ToString();
            exp = exp.Substring(0, exp.Length - " && ".Length);
            var options = ScriptOptions.Default.AddReferences(typeof(T).Assembly);
            Expression<Func<T, bool>> predicate = await CSharpScript.EvaluateAsync<Expression<Func<T, bool>>>(exp, options);
            return await ReadWrite.Get(predicate);
        }
    }
}