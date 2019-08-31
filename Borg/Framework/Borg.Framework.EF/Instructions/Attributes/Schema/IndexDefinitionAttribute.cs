using System;
using System.ComponentModel;

namespace Borg.Framework.EF.Instructions.Attributes.Schema
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class PrimaryKeyDefinitionAttribute : UniqueIndexDefinitionAttribute
    {
        public PrimaryKeyDefinitionAttribute() : base()
        {
        }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class UniqueIndexDefinitionAttribute : IndexDefinitionAttribute
    {
        public UniqueIndexDefinitionAttribute() : base()
        {
        }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class IndexDefinitionAttribute : Attribute
    {
        public IndexDefinitionAttribute()
        {
        }

        public const string DefaultIndexName = nameof(IndexName);
        public string IndexName { get; set; } = DefaultIndexName;

        [DefaultValue(0)]
        public int Order { get; set; } = 0;

        public enum IndexDefinitionMode
        {
            Index, UniqueIndex, PrimaryKey
        }
    }
}