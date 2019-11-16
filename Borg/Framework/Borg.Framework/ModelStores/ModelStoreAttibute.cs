using Borg.Infrastructure.Core;
using Borg.Infrastructure.Core.DDD.Contracts;
using Borg.Infrastructure.Core.Reflection.Discovery;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Borg.Framework.Cms.Contracts
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class ModelStoreAttibute : Attribute
    {

    }
    public class ModelStoreAssemblyScanResult : AssemblyScanResult
    {
        public ModelStoreAssemblyScanResult(Assembly assembly, List<Type> modelStoreTypes) : base(assembly, true, new string[0])
        {
            ModelStoreTypes = Preconditions.NotEmpty(modelStoreTypes, nameof(modelStoreTypes));
        }

        public ModelStoreAssemblyScanResult(Assembly assembly, string[] errors) : base(assembly, false, errors)
        {
            ModelStoreTypes = null;
        }
        public IEnumerable<Type> ModelStoreTypes { get; }
    }

    public class ModelStoreAssemblyScanner : AssemblyScanner<ModelStoreAssemblyScanResult>, IDisposable
    {
        private readonly List<Type> modelStoreTypes;
        private ModelStoreAssemblyScanResult Result;
        public ModelStoreAssemblyScanner(Assembly assembly, ILoggerFactory loggerfactory) : base(loggerfactory, assembly)
        {
            modelStoreTypes = new List<Type>();


            Populate();
        }

        private void Populate()
        {
            var types = Assembly.GetTypes().Where(t => t.IsModelStoreType());
            if (!types.Any())
            {
                Result = new ModelStoreAssemblyScanResult(Assembly, new string[] { $"No Model Store types in {Assembly.FullName}" });
                return;
            }

            modelStoreTypes.AddRange(types);


            Result = new ModelStoreAssemblyScanResult(Assembly, modelStoreTypes);
        }
        protected override Task<ModelStoreAssemblyScanResult> ScanInternal()
        {
            return Task.FromResult(Result);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~ApplicationSettingsAssemblyScanner()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }


        #endregion
    }

    internal static class HelperExtensions
    {
        public static bool IsModelStoreType(this Type type)
        {
            return type.ImplementsInterface<IIdentifiable>() && type.GetCustomAttribute<ModelStoreAttibute>() != null && (!(type.IsAbstract || type.IsInterface));
        }
    }
}
