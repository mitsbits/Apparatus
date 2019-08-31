using Borg.Infrastructure.Core.Reflection.Discovery;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;

namespace Borg.Framework.Reflection.Services
{
    public class FileAssemblyProvider : AssemblyProvider
    {
        private readonly string _path;

        public FileAssemblyProvider(ILoggerFactory loggerFactory, string path, Func<Assembly, bool> predicate = null) : base(loggerFactory, predicate)
        {
            _path = path;
        }

        protected override IEnumerable<Assembly> Candidates()
        {
            var source = new List<Assembly>();
            GetAssembliesFromPath(source, _path);
            return source;
        }

        private void GetAssembliesFromPath(List<Assembly> assemblies, string path)
        {
            if (!string.IsNullOrEmpty(path) && File.Exists(path))
            {
                Logger.Info("Discovering and loading assemblies from path '{0}'", path);

                try
                {
                    var assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(path);

                    assemblies.Add(assembly);
                    Logger.Info("Assembly '{0}' is discovered and loaded", assembly.FullName);
                }
                catch (Exception e)
                {
                    Logger.Warn("Error loading assembly '{0}'", path);
                    Logger.Warn(e.ToString());
                }
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