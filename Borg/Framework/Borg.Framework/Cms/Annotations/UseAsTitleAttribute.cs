using System;

namespace Borg.Framework.Cms
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class UseAsTitleAttribute : Attribute
    {
    }
}