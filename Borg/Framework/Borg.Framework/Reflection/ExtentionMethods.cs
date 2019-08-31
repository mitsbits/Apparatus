using Borg.Framework.Cms.Annotations;
using Borg.Infrastructure.Core.Reflection.Discovery.ObjectGraph;
using Microsoft.Extensions.Logging;
using System;
using System.Reflection;

namespace Borg
{
    public static class ExtentionMethods
    {
        public static bool IsCmsAggregateRoot(this Type type)
        {
            if (type == null) return false;
            if (type.IsAbstract) return false;
            return HasAggregateRootAttribute(type);
        }

        public static ComplexTypeRecursorResult RecurseForComplexTypes(this Type type, ILogger logger = null)
        {
            using (var recursor = new ComplexTypeRecursor(type, null, logger))
            {
                return recursor.Results();
            }
        }

        #region Private

        private static bool HasAggregateRootAttribute(Type type)
        {
            var attrs = type.GetCustomAttributes();
            foreach (var attr in attrs)
            {
                if (typeof(CmsAggregateRootAttribute).IsAssignableFrom(attr.GetType())) return true;
            }
            return false;
        }

        #endregion Private
    }
}