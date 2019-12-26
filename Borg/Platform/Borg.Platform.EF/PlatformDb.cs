using Borg.Framework.EF;
using Borg.Infrastructure.Core.Reflection.Discovery;
using Borg.Platform.EF.ContentBlocks;
using Borg.Platform.EF.Silos;
using Microsoft.EntityFrameworkCore;

namespace Borg.Platform.EF
{
    public class PlatformDb : BorgDbContext
    {
        public PlatformDb() : base()
        {
        }

        internal PlatformDb(DbContextOptions<PlatformDb> options) : base(options)
        {
        }

        internal PlatformDb(DbContextOptions<PlatformDb> options, IAssemblyExplorerResult explorerResult) : base(options, explorerResult)
        {
        }

        public DbSet<Language> Languages { get; set; }
        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Page> Pages { get; set; }
    
    }
}