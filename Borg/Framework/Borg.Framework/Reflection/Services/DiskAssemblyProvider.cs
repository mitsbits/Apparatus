using Borg.Infrastructure.Core.Reflection.Discovery;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;

namespace Borg.Framework.Reflection.Services
{
    public class DiskAssemblyProvider : AssemblyProvider
    {
        private readonly string _path;
        private readonly bool _includingSubpaths;

        public DiskAssemblyProvider(ILoggerFactory loggerFactory, string path, bool includingSubpaths = false, Func<Assembly, bool> predicate = null) : base(loggerFactory, predicate)
        {
            _path = path;
            _includingSubpaths = includingSubpaths;
        }

        protected override IEnumerable<Assembly> Candidates()
        {
            var source = new List<Assembly>();
            GetAssembliesFromPath(source, _path, _includingSubpaths);
            return source;
        }

        private void GetAssembliesFromPath(List<Assembly> assemblies, string path, bool includingSubpaths)
        {
            if (!string.IsNullOrEmpty(path) && Directory.Exists(path))
            {
                Logger.Info("Discovering and loading assemblies from path '{0}'", path);

                foreach (var extensionPath in Directory.EnumerateFiles(path, "*.dll"))
                {
                    try
                    {
                        var assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(extensionPath);

                        assemblies.Add(assembly);
                        Logger.Info("Assembly '{0}' is discovered and loaded", assembly.FullName);
                    }
                    catch (Exception e)
                    {
                        Logger.Warn("Error loading assembly '{0}'", extensionPath);
                        Logger.Warn(e.ToString());
                    }
                }

                if (includingSubpaths)
                    foreach (string subpath in Directory.GetDirectories(path))
                        GetAssembliesFromPath(assemblies, subpath, true);
            }
            else
            {
                Logger.Warn(
                    string.IsNullOrEmpty(path)
                        ? "Discovering and loading assemblies from path skipped: path not provided"
                        : "Discovering and loading assemblies from path '{0}' skipped: path not found", path);
            }
        }
    }
}