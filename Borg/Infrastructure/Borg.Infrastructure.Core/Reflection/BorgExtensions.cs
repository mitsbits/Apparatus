using Borg.Infrastructure.Core;
using System.Linq;
using System.Reflection;

namespace Borg
{
    public static class BorgExtensions
    {
        /// <summary>
        /// Checks to see if the assembly is part of the Borg Hive
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static bool Assimilated(this Assembly assembly)
        {
            return assembly.GetTypes().Any(x => x.IsAssignableTo(typeof(IResistanceIsFutile)));
        }
    }
}