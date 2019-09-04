using Borg.Infrastructure.Core;
using System;

namespace Borg.Framework.Cms
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class PropertyTabAttribute : Attribute
    {
        public const string DefaultTabHeader = "General";
        public string Header { get; set; } = DefaultTabHeader;

        public PropertyTabAttribute()
        {
        }

        public PropertyTabAttribute(string header)
        {
            Header = Preconditions.NotEmpty(header, nameof(header));
        }
    }
}