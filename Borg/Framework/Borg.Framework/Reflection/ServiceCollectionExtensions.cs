using Borg.Framework.Reflection.Discovery;
using Borg.Framework.Reflection.Discovery.AssemblyScanner;
using Borg.Framework.Reflection.ServiceRegistry;
using Borg.Framework.Reflection.Services;
using Borg.Infrastructure.Core.DI;
using Borg.Infrastructure.Core.Reflection.Discovery;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterPlugableServices(this IServiceCollection services, ILoggerFactory loggerFactory, params IAssemblyProvider[] assemblyProviders)
        {
            var explorer = new PlugableServicesExplorer(loggerFactory, assemblyProviders);
            if (!explorer.ScanCompleted) explorer.Scan();
            var results = explorer.Results().Cast<PlugableServicesAssemblyScanResult>();
            foreach (var result in results)
            {
                if (!result.Success) continue;
                foreach (var contract in result.Instructions.SelectMany(x => x.ImplementedInterfaces).Distinct())
                {
                    var registrations = result.Instructions.Where(x => x.ImplementedInterfaces.Any(i => i == contract));
                    RegisterServices(contract, registrations.ToArray(), services);
                }
            }
            return services;
        }

        private static void RegisterServices(Type contract, IEnumerable<PlugableServicesAssemblyScanResult.Instruction> registrations, IServiceCollection services)
        {
            var isMultipleRegistration = registrations.SelectMany(x => x.Attributes.Where(a => a.ImplementationOf == contract)).All(a => a.OneOfMany);
            var filterdRegistations = registrations.Where(x => x.ImplementedInterfaces.Any(i => i == contract)).Select(x => (service: x.Service, attr: x.Attributes.First(a => a.ImplementationOf == contract)));

            if (isMultipleRegistration)
            {
                foreach (var registry in filterdRegistations.OrderBy(x => x.attr.Order))
                {
                    services.Add(new ServiceDescriptor(contract, registry.service,
                        registry.attr.Lifetime == Lifetime.Transient
                        ? ServiceLifetime.Transient : registry.attr.Lifetime == Lifetime.Scoped
                        ? ServiceLifetime.Scoped : ServiceLifetime.Singleton));
                }
            }
            else
            {
                var registry = filterdRegistations.OrderBy(x => x.attr.Order).Last();
                services.Add(new ServiceDescriptor(contract, registry.service,
                    registry.attr.Lifetime == Lifetime.Transient
                    ? ServiceLifetime.Transient : registry.attr.Lifetime == Lifetime.Scoped
                    ? ServiceLifetime.Scoped : ServiceLifetime.Singleton));
            }
        }

        public static IServiceCollection AddAssmblyScanners(this IServiceCollection services, ILoggerFactory loggerFactory)
        {
            var asmblProvider = new DepedencyAssemblyProvider(loggerFactory);
            new ServiceDiscovery(asmblProvider.GetAssemblies().ToArray(), services);
            return services;
        }
    }
}