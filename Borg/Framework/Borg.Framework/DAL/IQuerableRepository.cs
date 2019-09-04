using System.Linq;

namespace Borg.Framework.DAL
{
    public interface IQuerableRepository<T> : IRepository<T> where T : class
    {
        IQueryable<T> Query { get; }
    }
}