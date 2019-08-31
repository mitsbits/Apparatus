using Borg.Infrastructure.Core;
using Borg.Infrastructure.Core.Reflection.Discovery;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Borg
{
    public static class IAssemblyExplorerResultExtensions
    {
        public static IEnumerable<TResult> Results<TResult>(this IAssemblyExplorerResult orchestrator, Func<AssemblyScanResult, bool> predicate = null) where TResult : AssemblyScanResult
        {
            orchestrator = Preconditions.NotNull(orchestrator, nameof(orchestrator));
            var query = orchestrator.Results;
            if (predicate != null) query = query.Where(predicate);
            foreach (var result in query)
            {
                if (result.GetType().Equals(typeof(TResult))) yield return result as TResult;
            }
        }
    }
}