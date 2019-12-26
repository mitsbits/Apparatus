using Borg.Framework.Cms.BuildingBlocks;
using Borg.Infrastructure.Core.DDD.Contracts;
using Borg.Infrastructure.Core.DDD.ValueObjects;
using Borg.Platform.EF.Instructions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Borg.Platform.EF.Silos
{
    public class Language : ILanguage, IIdentifiable, IDataState
    {
        public virtual int Id { get; protected set; }
        public virtual string TwoLetterISOCode { get; protected set; } = string.Empty;

        public virtual CompositeKey Keys => CompositeKeyBuilder.CreateWithFieldName(nameof(Id)).AddValue(Id).Build();
    }

    public class LanguageInstruction<TDbContext> : EntityMap<Language, TDbContext> where TDbContext : DbContext
    {
        private readonly string seqName;

        public LanguageInstruction()
        {
            seqName = GetSequenceName(nameof(Language.Id));
        }

        public override void ConfigureDb(ModelBuilder builder)
        {
            base.ConfigureDb(builder);
            builder.HasSequence<int>(seqName);
        }

        public override void ConfigureEntity(EntityTypeBuilder<Language> builder)
        {
            base.ConfigureEntity(builder);
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasDefaultValueSql($"NEXT VALUE FOR {seqName}");
            builder.Property(x => x.TwoLetterISOCode).HasMaxLength(2).IsUnicode(false).IsRequired();
        }
    }
}