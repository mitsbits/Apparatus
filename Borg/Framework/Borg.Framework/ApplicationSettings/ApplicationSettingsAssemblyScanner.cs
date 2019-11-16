using Borg.Infrastructure.Core.Reflection.Discovery;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace Borg.Framework.ApplicationSettings
{
    public class ApplicationSettingsAssemblyScanner : AssemblyScanner<ApplicationSettingsAssemblyScanResult>, IDisposable
    {
        private readonly List<Type> applicationSettingTypes;
        private ApplicationSettingsAssemblyScanResult Result;
        public ApplicationSettingsAssemblyScanner(Assembly assembly, ILoggerFactory loggerfactory) : base(loggerfactory, assembly)
        {
            applicationSettingTypes = new List<Type>();


            Populate();
        }

        private void Populate()
        {
            var settings = Assembly.GetTypes().Where(t => t.IsApplicationSetting());
            if (!settings.Any())
            {
                Result = new ApplicationSettingsAssemblyScanResult(Assembly, new string[] { $"No {nameof(IApplicationSetting)} types in {Assembly.FullName}" });
                return;
            }

            applicationSettingTypes.AddRange(settings);


            Result = new ApplicationSettingsAssemblyScanResult(Assembly, applicationSettingTypes);
        }
        protected override Task<ApplicationSettingsAssemblyScanResult> ScanInternal()
        {
            throw new NotImplementedException();
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
        public static bool IsApplicationSetting(this Type type)
        {
            return type.ImplementsInterface<IApplicationSetting>() && type.IsSealed && (!(type.IsAbstract || type.IsInterface));
        }
    }
}
