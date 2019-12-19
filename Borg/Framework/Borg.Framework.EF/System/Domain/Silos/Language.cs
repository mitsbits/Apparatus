﻿using Borg.Framework.Cms.BuildingBlocks;
using Borg.Infrastructure.Core.DDD.Contracts;
using Borg.Infrastructure.Core.DDD.ValueObjects;
using Borg.Platform.EF.Instructions;
using Microsoft.EntityFrameworkCore;

namespace Borg.Framework.EF.System.Domain.Silos
{
    public class Language : ILanguage, IIdentifiable, IDataState
    {
        public virtual int Id { get; protected set; }
        public virtual string TwoLetterISOCode { get; protected set; } = string.Empty;

        public virtual CompositeKey Keys => CompositeKeyBuilder.CreateWithFieldName(nameof(Id)).AddValue(Id).Build();
    }

    public class LanguageInstruction<TDbContext> : EntityMap<Language, TDbContext> where TDbContext : DbContext
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