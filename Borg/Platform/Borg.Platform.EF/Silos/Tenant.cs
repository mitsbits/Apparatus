using Borg.Framework.Cms.BuildingBlocks;
using Borg.Infrastructure.Core.DDD.Contracts;
using Borg.Infrastructure.Core.DDD.ValueObjects;
using Borg.Platform.EF.Instructions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Borg.Platform.EF.Silos
{
    public class Tenant : ITenant, IIdentifiable, IDataState
    {
        public virtual int Id { get; protected set; }
        public string Name { get; protected set; }
        public string Description { get; protected set; }

        public virtual CompositeKey Keys => CompositeKeyBuilder.CreateWithFieldName(nameof(Id)).AddValue(Id).Build();
    }

    public class TenantInstruction<TDbContext> : EntityMap<Tenant, TDbContext> where TDbContext : DbContext
    {
        private readonly string seqName;

        public TenantInstruction()
        {
            seqName = GetSequenceName(nameof(Tenant.Id));
        }

        public override void ConfigureDb(ModelBuilder builder)
        {
            base.ConfigureDb(builder);
            builder.HasSequence<int>(seqName);
        }

        public override void ConfigureEntity(EntityTypeBuilder<Tenant> builder)
        {
            base.ConfigureEntity(builder);
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasDefaultValueSql($"NEXT VALUE FOR {seqName}");
            builder.Property(x => x.Name).HasMaxLength(50).IsUnicode(false).IsRequired();
            builder.Property(x => x.Description).IsUnicode(true).IsRequired(false);
        }
    }
}