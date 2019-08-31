using System;

namespace Borg.Framework.Cms.Annotations
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class CmsAggregateRootAttribute : Attribute
    {
        public string Plural { get; set; }
        public string Singular { get; set; }
    }
}