using Borg.Infrastructure.Core.Reflection.Discovery.Annotations;
using System;
using System.Reflection;

namespace Borg
{
    public static class MapperIgnoreAttributeExtensions
    {
        public static bool IsMapperIgnore(this PropertyInfo type)
        {
            return type.PropertyType.IsMapperIgnore();
        }

        public static bool IsMapperIgnore(this TypeInfo type)
        {
            return type.HasAttribute<MapperIgnoreAttribute>();
        }

        public static bool IsMapperIgnore(this Type type)
        {
            return type.GetTypeInfo().IsMapperIgnore();
        }
    }
}