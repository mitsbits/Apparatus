using Microsoft.EntityFrameworkCore;

namespace Borg.Framework.EF.Contracts
{
    public interface IHaveDbContext<out TDbContext> where TDbContext : DbContext
    {
        TDbContext Context { get; }
    }
}