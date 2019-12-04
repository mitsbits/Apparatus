using System.Linq;
using System.Reflection;

namespace Borg.Infrastructure.Core.DDD.ValueObjects
{
    internal static class ExcludeValueObjectExtensions
    {
        public static bool IsExcludedFromValueObjectComparison(this PropertyInfo propertyInfo)
        {
            return propertyInfo.GetMethod.IsPrivate
                ? true
                : propertyInfo.GetCustomAttributes().Any(x => x.GetType().IsSubclassOf(typeof(ExcludeFromValueObjectAttribute)));
        }
    }
}