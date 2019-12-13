using Borg.Framework.Cms.BuildingBlocks;
using Borg.Platform.EF.Instructions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Borg.Platform.EF.Silos
{
    public class Tenant : ITenant
    {
        public virtual int Id { get; protected set; }
        public string Name { get; protected set; }
    }

    public class TenantInstruction : EntityMap<Tenant, PlatformDb>
    {
        public override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            var seqName = GetSequenceName(nameof(Tenant.Id));
            builder.HasSequence<int>(seqName);
            builder.Entity<Tenant>().HasKey(x => x.Id);
            builder.Entity<Tenant>().Property(x => x.Id).HasDefaultValueSql($"NEXT VALUE FOR {seqName}");
            builder.Entity<Tenant>().Property(x => x.Name).HasMaxLength(50).IsUnicode(false).IsRequired();
        }
    }
}
