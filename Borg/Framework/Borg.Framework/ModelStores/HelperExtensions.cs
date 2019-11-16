using Borg.Infrastructure.Core.DDD.Contracts;
using System;
using System.Reflection;

namespace Borg.Framework.Cms.Contracts
{
    internal static class HelperExtensions
    {
        public static bool IsModelStoreType(this Type type)
        {
            return type.ImplementsInterface<IIdentifiable>() && type.GetCustomAttribute<ModelStoreAttibute>() != null && (!(type.IsAbstract || type.IsInterface));
        }
    }
}
