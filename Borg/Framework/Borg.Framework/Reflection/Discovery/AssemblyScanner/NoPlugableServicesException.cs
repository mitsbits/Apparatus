using System;
using System.Reflection;

namespace Borg.Framework.Reflection.Discovery.AssemblyScanner
{
    internal class NoPlugableServicesException : ApplicationException
    {
        public NoPlugableServicesException(Assembly assembly) : base(BuildMessage(assembly))
        {
        }

        private static string BuildMessage(Assembly assembly)
        {
            return $"Assembly {assembly.GetName().Name} has no Plugable Services defined";
        }
    }
}