using Borg.Framework.DAL;
using Borg.Framework.DAL.Ordering;
using Borg.Infrastructure.Core;
using Borg.Infrastructure.Core.Collections;
using Borg.Platform.EF.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Borg.Framework.EF.DAL
{
    public class ReadRepository<T, TDbContext> : IReadRepository<T> where T : class where TDbContext : DbContext
    {
        public ReadRepository(TDbContext dbContext)
        {
            Context = Preconditions.NotNull(dbContext, nameof(dbContext));
            if (!Context.EntityIsMapped<T, TDbContext>()) throw new EntityNotMappedException<TDbContext>(typeof(T));
        }

        protected TDbContext Context { get; }

        public async Task<IPagedResult<T>> Read(Expression<Func<T, bool>> predicate, int page, int records, IEnumerable<OrderByInfo<T>> orderBy, CancellationToken cancellationToken = default(CancellationToken), params Expression<Func<T, dynamic>>[] paths)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await Context.Fetch(predicate, page, records, orderBy, cancellationToken, false, paths);
        }
    }

    public class ReadRepository<T> : IReadRepository<T> where T : class
    {
        public ReadRepository(DbContext dbContext)
        {
            Context = Preconditions.NotNull(dbContext, nameof(dbContext));
            if (!Context.EntityIsMapped<T>()) throw new EntityNotMappedException<DbContext>(typeof(T));
        }

        protected DbContext Context { get; }

        public async Task<IPagedResult<T>> Read(Expression<Func<T, bool>> predicate, int page, int records, IEnumerable<OrderByInfo<T>> orderBy, CancellationToken cancellationToken = default(CancellationToken), params Expression<Func<T, dynamic>>[] paths)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await Context.Fetch(predicate, page, records, orderBy, cancellationToken, false, paths);
        }
    }
}