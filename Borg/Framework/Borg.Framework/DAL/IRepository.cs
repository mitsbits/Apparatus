namespace Borg.Framework.DAL
{
    public interface IRepository //marker
    {
    }

    public interface IRepository<T> : IRepository //marker
    {
    }
}