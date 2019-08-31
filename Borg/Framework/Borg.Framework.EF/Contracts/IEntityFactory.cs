using System.Threading.Tasks;

namespace Borg.Framework.EF.Contracts
{
    public interface IEntityFactory
    {
        Task<TEntity> Instance<TEntity>() where TEntity : class;
    }
}