using Borg.Framework.EF;
using Borg.Framework.EF.System.Domain.Silos;
using Microsoft.EntityFrameworkCore;

namespace Borg.Platform.EF
{
    public class PlatformDb : BorgDbContext
    {
        public DbSet<Language> Languages { get; set; }
        public DbSet<Tenant> Tenants { get; set; }
    }
}