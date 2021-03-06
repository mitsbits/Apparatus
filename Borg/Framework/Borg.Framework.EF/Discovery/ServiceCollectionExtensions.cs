﻿using Borg.Framework.EF.Discovery.AssemblyScanner;
using Borg.Infrastructure.Core;
using Borg.Infrastructure.Core.Reflection.Discovery;
using System.Collections.Generic;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        public static IServiceCollection BorgDbAssemblyScan(this IServiceCollection services, IEnumerable<IAssemblyProvider> providers, out AssemblyExplorerResult explorerResult)
        {
            var explorer = new BorgDbAssemblyExplorer(null, Preconditions.NotEmpty(providers, nameof(providers)));
            explorerResult = new AssemblyExplorerResult(null, new[] { explorer });
            return services.AddSingleton<IAssemblyExplorerResult>(explorerResult);
        }
    }
}