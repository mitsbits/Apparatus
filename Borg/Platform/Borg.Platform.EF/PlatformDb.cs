using Borg.Framework.EF;
using Borg.Framework.EF.System.Domain.Silos;
using Borg.Framework.EF.System.Domain.System;
using Borg.Platform.EF.SystemEntites;
using Microsoft.EntityFrameworkCore;

namespace Borg.Platform.EF
{
    public class PlatformDb : BorgDbContext
    {
        public DbSet<Language> Languages { get; set; }
        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Page> Pages { get; set; }
        public DbSet<Folder> Dictionaries { get; set; }
    }
}