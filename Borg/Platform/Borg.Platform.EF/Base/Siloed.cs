using Borg.Infrastructure.Core.DDD.Contracts;
using Borg.Infrastructure.Core.DDD.ValueObjects;
using Borg.Platform.EF.Instructions;
using Borg.Platform.EF.Silos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Borg.Platform.EF.Base
{
    public abstract class Siloed : IIdentifiable, IDataState
    {
        public virtual int Id { get; protected set; }
        public virtual int LanguageId { get; protected set; }
        public virtual int TenantId { get; protected set; }

        public virtual Language? Language { get; protected set; }
        public virtual Tenant? Tenant { get; protected set; }

        public virtual CompositeKey Keys => CompositeKeyBuilder.CreateWithId(Id)
            .AddKey(nameof(LanguageId)).AddValue(LanguageId)
            .AddKey(nameof(TenantId)).AddValue(TenantId)
            .Build();
    }

    public abstract class SiloedInstruction<T, TDbContext> : EntityMap<T, TDbContext> where T : Siloed where TDbContext : DbContext
    {
        public override void ConfigureDb(ModelBuilder builder)
        {
            base.ConfigureDb(builder);
            var seqName = GetSequenceName(nameof(Siloed.Id));
            builder.HasSequence<int>(seqName);
        }

        public override void ConfigureEntity(EntityTypeBuilder<T> builder)
        {
            base.ConfigureEntity(builder);
            builder.HasKey(x => new { x.Id, x.TenantId, x.LanguageId });
            builder.HasOne<Tenant>(x => x.Tenant);
            builder.HasOne<Language>(x => x.Language);
        }
    }
}