using Borg.Infrastructure.Core.DDD.Contracts;
using System;
using System.Reflection;

namespace Borg.Framework.EF.Discovery
{
    internal static class ExtensionMethods
    {
        internal static TypeInfo _borgDbType = typeof(BorgDbContext).GetTypeInfo();
        internal static TypeInfo _dataStateType = typeof(IDataState).GetTypeInfo();

        public static bool IsBorgDb(this Type type)
        {
            return type.GetTypeInfo().IsBorgDb();
        }

        public static bool IsBorgDb(this TypeInfo type)
        {
            return type == null || type.IsAbstract ? false : type.IsSubclassOf(_borgDbType);
        }

        public static bool IsDataState(this Type type)
        {
            return type.GetTypeInfo().IsDataState();
        }

        public static bool IsDataState(this TypeInfo type)
        {
            return type == null || type.IsAbstract ? false : type.ImplementsInterface(_dataStateType);
        }
    }
}