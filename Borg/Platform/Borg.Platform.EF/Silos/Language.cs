using Borg.Framework.Cms.BuildingBlocks;
using Borg.Platform.EF.Instructions;
using Microsoft.EntityFrameworkCore;

namespace Borg.Platform.EF.Silos
{
    public class Language : ILanguage
    {
        public virtual int Id { get; protected set; }
        public virtual string TwoLetterISOCode { get; protected set; }
    }

    public class LanguageInstruction : EntityMap<Language, PlatformDb>
    {
        public override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            var seqName = GetSequenceName(nameof(Language.Id));
            builder.HasSequence<int>(seqName);
            builder.Entity<Language>().HasKey(x => x.Id);
            builder.Entity<Language>().Property(x => x.Id).HasDefaultValueSql($"NEXT VALUE FOR {seqName}");
            builder.Entity<Language>().Property(x => x.TwoLetterISOCode).HasMaxLength(2).IsUnicode(false).IsRequired();
        }
    }
}