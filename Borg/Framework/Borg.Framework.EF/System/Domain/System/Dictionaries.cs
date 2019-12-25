using Borg.Framework.EF.System.Domain.Silos;
using Borg.Infrastructure.Core;
using Borg.Infrastructure.Core.DDD.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;

namespace Borg.Framework.EF.System.Domain.System
{
    public abstract class DictionaryBase : Treenode
    {
        internal const string DictionariesTableName = "Dictionaries";
        private EntryType type;

        protected DictionaryBase(EntryType type)
        {
            this.type = type;
        }

        protected DictionaryBase()
        {
      
        }


        public enum EntryType
        {
            Folder,
            KeyValuePair
        }
    }

    public class Folder : DictionaryBase, IHaveName
    {
        public Folder(string name) : base(EntryType.Folder)
        {
            Name = Preconditions.NotEmpty(name, nameof(name));
        }

        public virtual string Name { get; protected set; }

        public virtual ICollection<Entry> Entries { get; protected set; } = new HashSet<Entry>();
        public virtual ICollection<Folder> Folders { get; protected set; } = new HashSet<Folder>();
        public virtual Folder? Parent { get; protected set; }
    }

    public class Entry : DictionaryBase, IKeyValuePair<string, string>
    {
        public Entry(string field, string value) : base(EntryType.KeyValuePair)
        {
            Key = Preconditions.NotEmpty(field, nameof(field));
            Value = value;
        }

        protected Entry():base()
        {

        }

        public string Key { get; protected set; }

        public string Value { get; protected set; }

        public virtual Folder? Parent { get; protected set; }
    }

    public class FolderInstruction<TDbContext> : TreenodeInstruction<Folder, TDbContext> where TDbContext : DbContext
    {
        public override void ConfigureDb(ModelBuilder builder)
        {
            base.ConfigureDb(builder);

        }

        public override void ConfigureEntity(EntityTypeBuilder<Folder> builder)
        {
            base.ConfigureEntity(builder);
            builder.ToTable(DictionaryBase.DictionariesTableName);
            builder.Property(x => x.Name).IsRequired().IsUnicode().HasMaxLength(1024);
            builder.HasMany(x => x.Folders).WithOne(x => x.Parent).HasForeignKey(x => x.ParentId);
        }
    }

    public class EntryInstruction<TDbContext> : TreenodeInstruction<Entry, TDbContext> where TDbContext : DbContext
    {
        public override void ConfigureDb(ModelBuilder builder)
        {
            base.ConfigureDb(builder);
           

        }

    }
}