using Borg.Framework.DAL.Ordering;
using Borg.Infrastructure.Core.Collections;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Borg.Framework.DAL
{
    public interface ISearchRepository<T> : IRepository<T> where T : class
    {
        Task<IPagedResult<T>> Find(string term, int page, int size,
               IEnumerable<OrderByInfo<T>> orderByy, CancellationToken cancellationToken = default,
               params Expression<Func<T, dynamic>>[] paths);
    }
}