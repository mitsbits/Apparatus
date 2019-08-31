using Borg.Framework.DAL;
using Microsoft.EntityFrameworkCore;

namespace Borg.Framework.EF.Contracts
{
    public interface IUnitOfWork<out TDbContext> : IUnitOfWork, IEntityFactory where TDbContext : DbContext
    {
    }
}