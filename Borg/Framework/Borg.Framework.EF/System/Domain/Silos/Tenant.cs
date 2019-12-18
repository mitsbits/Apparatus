using Borg.Framework.Cms.BuildingBlocks;
using Borg.Infrastructure.Core.DDD.Contracts;
using Borg.Infrastructure.Core.DDD.ValueObjects;
using Borg.Platform.EF.Instructions;
using Microsoft.EntityFrameworkCore;

namespace Borg.Framework.EF.System.Domain.Silos
{
    public class Tenant : ITenant, IIdentifiable, IDataState
    {
        public virtual int Id { get; protected set; }
        public string Name { get; protected set; }
        public string Description { get; protected set; }

        public virtual CompositeKey Keys => CompositeKeyBuilder.CreateWithFieldName(nameof(Id)).AddValue(Id).Build();
    }

    public class TenantInstruction<TDbContext> : EntityMap<Tenant, TDbContext> where TDbContext : DbContext;
    {
        public override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            var seqName = GetSequenceName(nameof(Tenant.Id));
            builder.HasSequence<int>(seqName);
            builder.Entity<Tenant>().HasKey(x => x.Id);
            builder.Entity<Tenant>().Property(x => x.Id).HasDefaultValueSql($"NEXT VALUE FOR {seqName}");
            builder.Entity<Tenant>().Property(x => x.Name).HasMaxLength(50).IsUnicode(false).IsRequired();
            builder.Entity<Tenant>().Property(x => x.Description).IsUnicode(true).IsRequired(false);
        }
    }
}