namespace Borg.Framework.DAL.Ordering
{
    public interface ICanAddOrderBys<T> where T : class
    {
        ICanAddAndBuildOrderBys<T> Add(OrderByInfo<T> item);
    }
}