using System;
using System.ComponentModel;

namespace Borg.Framework.EF.Instructions.Attributes.Schema
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class PrimaryKeySequenceDefinitionAttribute : IndexSequenceDefinitionAttribute
    {
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = false)]
    public class IndexSequenceDefinitionAttribute : SequenceDefinitionAttribute
    {
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = false)]
    public class SequenceDefinitionAttribute : Attribute
    {
        [DefaultValue(1)]
        public int StartsAt { get; set; } = 1;

        [DefaultValue(1)]
        public int IncrementsBy { get; set; } = 1;
    }
}