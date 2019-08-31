using Borg.Infrastructure.Core.Reflection.Discovery;
using Microsoft.Extensions.DependencyModel;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;

namespace Borg.Framework.Reflection.Services
{
    public class DepedencyAssemblyProvider : AssemblyProvider
    {
        public DepedencyAssemblyProvider(ILoggerFactory loggerFactory, Func<Assembly, bool> predicate = null) : base(loggerFactory, predicate)
        {
        }

        protected override IEnumerable<Assembly> Candidates()
        {
            var source = new List<Assembly>();
            foreach (CompilationLibrary compilationLibrary in DependencyContext.Default.CompileLibraries)
            {
                try
                {
                    source.Add(AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(compilationLibrary.Name)));
                }
                catch (FileNotFoundException fnfex)
                {
                    Logger.Warn(fnfex.ToString());
                }
                catch (Exception ex)
                {
                    Logger.Warn(ex.ToString());
                }
            }
            return source;
        }
    }
}