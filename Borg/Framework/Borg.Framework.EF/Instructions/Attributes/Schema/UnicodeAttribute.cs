using System;
using System.Collections.Generic;
using System.Text;

namespace Borg.Framework.EF.Instructions.Attributes.Schema
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class UnicodeAttribute : Attribute
    {
        public UnicodeAttribute(bool isUnicode = false)
        {
            IsUnicode = IsUnicode;
        }
        public bool IsUnicode { get; }
    }
}
