using Borg.Framework.EF;
using Borg.Platform.EF.Silos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Borg.Platform.EF
{
   public class PlatformDb : BorgDbContext
    {
        public DbSet<Language> Languages { get; set; }
        public DbSet<Tenant> Tenants { get; set; }
    }
}
