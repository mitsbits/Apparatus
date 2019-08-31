using Borg.Infrastructure.Core.Reflection.Discovery;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

namespace Borg.Framework.Reflection.Services
{
    public class ReferenceAssemblyProvider : AssemblyProvider
    {
        private readonly IEnumerable<AssemblyName> _assemblies;

        public ReferenceAssemblyProvider(ILoggerFactory loggerFactory, Func<Assembly, bool> predicate = null, params Assembly[] assemblies) : base(loggerFactory, predicate)
        {
            if (assemblies == null || !assemblies.Any())
                _assemblies = (new[] { Assembly.GetExecutingAssembly().GetName() }.Union(Assembly.GetExecutingAssembly().GetReferencedAssemblies()));

            _assemblies = assemblies.Select(x => x.GetName());
        }

        protected override IEnumerable<Assembly> Candidates()
        {
            var source = new List<Assembly>();
            GetAssembliesFromReferenceContext(source);
            return source;
        }

        private void GetAssembliesFromReferenceContext(List<Assembly> assemblies)
        {
            Logger.Info("Discovering and loading assemblies from ReferenceContext");

            foreach (var asmbl in _assemblies)
            {
                Assembly assembly = null;

                try
                {
                    assembly = AssemblyLoadContext.Default.LoadFromAssemblyName(asmbl);

                    if (!assemblies.Any(a => string.Equals(a.FullName, assembly.FullName, StringComparison.OrdinalIgnoreCase)))
                    {
                        assemblies.Add(assembly);
                        Logger.Info("Assembly '{0}' is discovered and loaded", assembly.FullName);
                    }
                }
                catch (Exception e)
                {
                    Logger.Warn("Error loading assembly '{0}'", asmbl);
                    Logger.Warn(e.ToString());
                }
            }
        }
    }
}