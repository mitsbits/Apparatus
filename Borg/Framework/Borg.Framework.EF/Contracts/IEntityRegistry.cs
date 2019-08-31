using Microsoft.EntityFrameworkCore;

namespace Borg.Framework.EF.Contracts
{
    public interface IEntityRegistry
    {
        void RegisterWithDbContext(ModelBuilder builder);
    }
}