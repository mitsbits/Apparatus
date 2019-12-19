using Borg.Framework.EF.System.Domain.Silos;
using Borg.Infrastructure.Core;
using Borg.Infrastructure.Core.DDD.Contracts;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Borg.Framework.EF.System.Domain.System
{
    public abstract class DictionaryBase : Treenode
    {
        internal const string DictionariesTableName = "Dictionaries";
        EntryType type;
        protected DictionaryBase(EntryType type)
        {
            this.type = type;
        }

        public enum EntryType
        {
            Folder,
            KeyValuePair
        }
    }
    public class Folder : DictionaryBase, IHaveName
    {
        public Folder(string name):base(EntryType.Folder)
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

        public string Key { get; protected set; }

        public string Value { get; protected set; }

        public virtual Folder? Parent {get; protected set;}
    }

    public class FolderInstruction<TDbContext> : TreenodeInstruction<Folder, TDbContext> where TDbContext : DbContext
    {
        public override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Folder>().ToTable(DictionaryBase.DictionariesTableName);
            builder.Entity<Folder>().Property(x => x.Name).IsRequired().IsUnicode().HasMaxLength(1024);
            builder.Entity<Folder>().HasMany(x => x.Folders).WithOne(x => x.Parent).HasForeignKey(x => x.ParentId);
            builder.Entity<Folder>().HasMany(x => x.Entries).WithOne(x => x.Parent).HasForeignKey(x => x.ParentId);
        }
    }

    public class EntryInstruction<TDbContext> : TreenodeInstruction<Entry, TDbContext> where TDbContext : DbContext
    {
        public override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Entry>().ToTable(DictionaryBase.DictionariesTableName);
            builder.Entity<Entry>().Property(x => x.Key).IsRequired().IsUnicode().HasMaxLength(1024);
            builder.Entity<Entry>().Property(x => x.Value).IsRequired().IsUnicode().HasDefaultValue(string.Empty);
            builder.Entity<Entry>().HasOne(x => x.Parent).WithMany(x => x.Entries).HasForeignKey(x => x.ParentId);
        }
    }
}


