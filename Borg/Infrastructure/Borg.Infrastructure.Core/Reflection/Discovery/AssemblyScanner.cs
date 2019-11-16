using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Reflection;
using System.Threading.Tasks;

namespace Borg.Infrastructure.Core.Reflection.Discovery
{
    public abstract class AssemblyScanner<TResult> : IAssemblyScanner where TResult : AssemblyScanResult
    {
        protected Assembly Assembly { get; }
        protected ILogger Logger { get; }

        protected AssemblyScanner(ILoggerFactory loggerFactory, Assembly assembly)
        {
            Logger = loggerFactory.CreateForType(GetType());
            Assembly = Preconditions.NotNull(assembly, nameof(assembly));
        }

        public async Task<AssemblyScanResult> Scan()
        {
            Logger.Debug($"{GetType().Name} is about to scan assmbly {Assembly.GetName().Name}");
            var result = await ScanInternal();
            Logger.Debug($"{GetType().Name} scaned assmbly {Assembly.GetName().Name} and came up with results {result}");
            return result;
        }

        protected abstract Task<TResult> ScanInternal();
    }
}