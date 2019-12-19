using Borg.Infrastructure.Core.DDD.Contracts;
using System;
using System.Reflection;

namespace Borg.Framework.EF.Discovery.AssemblyScanner
{
    internal class NoAggregatesException : ApplicationException
    {
        public NoAggregatesException(Assembly assembly) : base(CreateExceptionMessage(assembly))
        {
        }

        private static string CreateExceptionMessage(Assembly assembly)
        {
            return $"Assembly {assembly.GetName().Name} has no Cms Aggregate Roots defined";
        }
    }

    internal class EmptyBorgDbScanResultException : ApplicationException
    {
        public EmptyBorgDbScanResultException(Assembly assembly) : base(CreateExceptionMessage(assembly))
        {
        }

        private static string CreateExceptionMessage(Assembly assembly)
        {
            return $"Assembly {assembly.GetName().Name} has no {nameof(BorgDbContext)} defined or any {nameof(IDataState)} implementations."
                + $"No Entity Maps where discovered.";
        }
    }
}