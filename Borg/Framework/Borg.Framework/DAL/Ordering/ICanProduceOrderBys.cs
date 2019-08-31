using System.Collections.Generic;

namespace Borg.Framework.DAL.Ordering
{
    public interface ICanProduceOrderBys<T> where T : class
    {
        IEnumerable<OrderByInfo<T>> Build();
    }
}