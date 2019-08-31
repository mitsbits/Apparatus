namespace Borg.Framework.DAL.Ordering
{
    public interface ICanAddAndBuildOrderBys<T> : ICanAddOrderBys<T>, ICanProduceOrderBys<T> where T : class
    {
    }
}