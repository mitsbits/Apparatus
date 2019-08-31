using System;

namespace Borg.Infrastructure.Core.Reflection.Discovery.Annotations
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class MapperIgnoreAttribute : Attribute
    {
    }
}