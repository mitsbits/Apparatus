using Borg.Framework.DAL;
using Borg.Framework.DAL.Ordering;
using Borg.Framework.EF.Contracts;
using Borg.Infrastructure.Core;
using Borg.Infrastructure.Core.Collections;
using Borg.Platform.EF.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Borg.Framework.EF.DAL
{
    public class QueryRepository<T, TDbContext> : IQueryRepository<T>, IQuerableRepository<T>, IHaveDbContext<TDbContext> where T : class where TDbContext : DbContext
    {
        public QueryRepository(TDbContext dbContext)
        {         
            Context = Preconditions.NotNull( dbContext, nameof(dbContext));
            if (!Context.EntityIsMapped<T, TDbContext>()) throw new EntityNotMappedException<TDbContext>(typeof(T));
        }

        public TDbContext Context { get; }

        public IQueryable<T> Query => Context.Set<T>().AsNoTracking();

        public async Task<IPagedResult<T>> Find(Expression<Func<T, bool>> predicate, int page, int records, IEnumerable<OrderByInfo<T>> orderBy, CancellationToken cancellationToken = default(CancellationToken), params Expression<Func<T, dynamic>>[] paths)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await Context.Fetch(predicate, page, records, orderBy, cancellationToken, true, paths);
        }
    }
}