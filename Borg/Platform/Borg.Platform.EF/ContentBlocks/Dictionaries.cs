using Borg.Infrastructure.Core;
using Borg.Infrastructure.Core.DDD.Contracts;
using Borg.Platform.EF.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Collections.Generic;
using static Borg.Platform.EF.ContentBlocks.DictionaryState;

namespace Borg.Platform.EF.ContentBlocks
{
    public class DictionaryState : Treenode, IHaveKey<string>, IHaveValue<string>
    {
        public string Key { get; protected set; } = string.Empty;
        public string Value { get; protected set; } = string.Empty;
        public DictionaryBlock Descriminator { get; protected set; } = DictionaryBlock.Folder;

        public enum DictionaryBlock
        {
            Folder = 1,
            Entry = 2
        }
    }

    public abstract class DictionaryBase : Treenode
    {
        internal const string DictionariesTableName = "Dictionaries";

        protected DictionaryBase(EntryTypes type)
        {
            EntryType = type;
        }

        protected DictionaryBase()
        {
            EntryType = EntryTypes.Folder;
        }

        public EntryTypes EntryType { get; protected set; }

        public enum EntryTypes
        {
            Folder,
            Entry
        }
    }

    public class Entry : DictionaryBase, IKeyValuePair<string, string>
    {
        public Entry(string field, string value) : base(EntryTypes.Entry)
        {
            Key = Preconditions.NotEmpty(field, nameof(field));
            Value = value;
        }

        protected Entry() : base()
        {
        }

        public string Key { get; protected set; }

        public string Value { get; protected set; }

        public virtual Folder? Parent { get; protected set; }
    }

    public class Folder : Entry
    {
        public Folder(string name) : base(name, string.Empty)
        {
            EntryType = EntryTypes.Folder;
        }

        protected Folder() : base()
        {
            EntryType = EntryTypes.Folder;
        }

        public virtual string Name { get; protected set; }

        public virtual ICollection<Entry> Entries { get; protected set; } = new HashSet<Entry>();
        public virtual ICollection<Folder> Folders { get; protected set; } = new HashSet<Folder>();
    }

    //public class FolderInstruction<TDbContext> : TreenodeInstruction<Folder, TDbContext> where TDbContext : DbContext
    //{
    //    public override void ConfigureDb(ModelBuilder builder)
    //    {
    //        base.ConfigureDb(builder);
    //    }

    //    public override void ConfigureEntity(EntityTypeBuilder<Folder> builder)
    //    {
    //        builder.HasBaseType<Entry>();

    //        //builder.HasMany(x=>x.Folders).WithOne()
    //        //    .HasForeignKey(x => new { x.TenantId, x.LanguageId, x.ParentId })
    //        //    .HasPrincipalKey(x => new { x.TenantId, x.LanguageId, x.Id })
    //        //    .OnDelete(DeleteBehavior.NoAction);
    //        //builder.HasMany(x => x.Entries).WithOne()
    //        //    .HasForeignKey(x => new { x.TenantId, x.LanguageId, x.ParentId })
    //        //    .HasPrincipalKey(x => new { x.TenantId, x.LanguageId, x.Id })
    //        //        .OnDelete(DeleteBehavior.NoAction);
    //        builder.Property(x => x.Name).HasColumnName(nameof(Entry.Key));
    //    }
    //}

    //public class EntryInstruction<TDbContext> : TreenodeInstruction<Entry, TDbContext> where TDbContext : DbContext
    //{
    //    public override void ConfigureDb(ModelBuilder builder)
    //    {
    //        base.ConfigureDb(builder);
    //    }

    //    public override void ConfigureEntity(EntityTypeBuilder<Entry> builder)
    //    {
    //        base.ConfigureEntity(builder);
    //        builder.ToTable(DictionaryBase.DictionariesTableName);
    //        builder.HasDiscriminator(x => x.EntryType).HasValue<Entry>(EntryTypes.Entry).HasValue<Folder>(EntryTypes.Folder);

    //        var converter = new EnumToStringConverter<EntryTypes>();
    //        builder.Property(x => x.EntryType).HasConversion(converter);

    //        builder.HasOne(x => x.Parent)
    //            .WithMany(x => x.Entries).HasForeignKey(x => new { x.TenantId, x.LanguageId, x.ParentId })
    //            .HasPrincipalKey(x => new { x.TenantId, x.LanguageId, x.Id })
    //            .OnDelete(DeleteBehavior.NoAction);
    //        builder.HasOne(x => x.Parent)
    //            .WithMany(x => x.Folders).HasForeignKey(x => new { x.TenantId, x.LanguageId, x.ParentId })
    //            .HasPrincipalKey(x => new { x.TenantId, x.LanguageId, x.Id })
    //            .OnDelete(DeleteBehavior.NoAction);
    //    }
    //}

    public class DictionaryStateInstruction<TDbContext> : TreenodeInstruction<DictionaryState, TDbContext> where TDbContext : DbContext
    {
        private readonly string seqName;

        public DictionaryStateInstruction()
        {
            seqName = GetSequenceName(nameof(DictionaryState.Id));
        }

        public override void ConfigureDb(ModelBuilder builder)
        {
            base.ConfigureDb(builder);
            builder.HasSequence<int>(seqName);
        }

        public override void ConfigureEntity(EntityTypeBuilder<DictionaryState> builder)
        {
            base.ConfigureEntity(builder);
            builder.Property(x => x.Key).HasMaxLength(512).IsUnicode(true);
            builder.Property(x => x.Value).HasMaxLength(int.MaxValue).IsUnicode(true);
            builder.Property(x => x.ParentId).IsRequired(false);

            var converter = new EnumToStringConverter<DictionaryBlock>();
            builder.Property(x => x.Descriminator).HasConversion(converter);
        }
    }
}