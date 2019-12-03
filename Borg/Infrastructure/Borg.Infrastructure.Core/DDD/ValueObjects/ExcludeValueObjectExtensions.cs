using System.Linq;
using System.Reflection;

namespace Borg.Infrastructure.Core.DDD.ValueObjects
{
    internal static class ExcludeValueObjectExtensions
    {
        public static bool IsExcludedFromValueObjectComparison(this FieldInfo fieldInfo)
        {
            return !fieldInfo.IsPrivate && fieldInfo.GetCustomAttributes().Any(x => x.GetType().IsSubclassOf(typeof(ExcludeFromValueObjectAttribute)));
        }
    }
}