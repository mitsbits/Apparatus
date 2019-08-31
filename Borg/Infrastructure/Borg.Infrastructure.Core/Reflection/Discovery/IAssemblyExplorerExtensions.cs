using Borg.Infrastructure.Core;
using Borg.Infrastructure.Core.Reflection.Discovery;
using System.Collections.Generic;

namespace Borg
{
    public static class IAssemblyExplorerExtensions
    {
        public static IEnumerable<AssemblyScanResult> ScanAndResult(this IAssemblyExplorer explorer)
        {
            explorer = Preconditions.NotNull(explorer, nameof(explorer));
            if (!explorer.ScanCompleted) explorer.Scan();
            return explorer.Results();
        }

        public static IEnumerable<TResult> SuccessResults<TResult>(this IAssemblyExplorer explorer) where TResult : AssemblyScanResult
        {
            explorer = Preconditions.NotNull(explorer, nameof(explorer));
            var results = explorer.ScanAndResult();
            foreach (var result in results)
            {
                if (result.GetType().Equals(typeof(TResult)) && result.Success) yield return result as TResult;
            }
        }
    }
}