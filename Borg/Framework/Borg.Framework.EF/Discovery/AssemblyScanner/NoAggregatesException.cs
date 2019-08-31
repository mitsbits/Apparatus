using System;
using System.Reflection;

namespace Borg.Framework.EF.Discovery.AssemblyScanner
{
    internal class NoAggregatesException : ApplicationException
    {
        public NoAggregatesException(Assembly assembly) : base(BuildMessage(assembly))
        {
        }

        private static string BuildMessage(Assembly assembly)
        {
            return $"Assembly {assembly.GetName().Name} has no Cms Aggregate Roots defined";
        }
    }
}