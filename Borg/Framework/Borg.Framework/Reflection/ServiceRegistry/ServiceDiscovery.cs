using Borg.Infrastructure.Core.DI;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Borg.Framework.Reflection.ServiceRegistry
{
    internal class ServiceDiscovery
    {
        private readonly List<(Type service, PlugableServiceAttribute[] attrs, Type[] interfaces)> _elligibleServices = new List<(Type service, PlugableServiceAttribute[] attrs, Type[] interfaces)>();

        internal ServiceDiscovery(Assembly[] assemblies, IServiceCollection services)
        {
            Discover(assemblies, services);
        }

        private void Discover(Assembly[] assemblies, IServiceCollection services)
        {
            _elligibleServices.AddRange(FindAllPlugableServices(assemblies));
            foreach (var contract in _elligibleServices.SelectMany(x => x.interfaces).Distinct())
            {
                var registrations = _elligibleServices.Where(x => x.interfaces.Any(i => i == contract));
                RegisterServices(contract, registrations, services);
            }
        }

        private void RegisterServices(Type contract, IEnumerable<(Type service, PlugableServiceAttribute[] attrs, Type[] interfaces)> registrations, IServiceCollection services)
        {
            var isMultipleRegistration = registrations.SelectMany(x => x.attrs.Where(a => a.ImplementationOf == contract)).All(a => a.OneOfMany);
            var filterdRegistations = _elligibleServices.Where(x => x.interfaces.Any(i => i == contract)).Select(x => (service: x.service, attr: x.attrs.First(a => a.ImplementationOf == contract)));
            var directory = new PlugableServiceRegistry();
            if (isMultipleRegistration)
            {
                foreach (var registry in filterdRegistations.OrderBy(x => x.attr.Order))
                {
                    services.Add(new ServiceDescriptor(contract, registry.service, (registry.attr.Lifetime == Lifetime.Transient ? ServiceLifetime.Transient : registry.attr.Lifetime == Lifetime.Scoped ? ServiceLifetime.Scoped : ServiceLifetime.Singleton)));
                    directory.Add(contract, registry.service, registry.attr);
                }
            }
            else
            {
                var registry = filterdRegistations.OrderBy(x => x.attr.Order).Last();
                services.Add(new ServiceDescriptor(contract, registry.service, (registry.attr.Lifetime == Lifetime.Transient ? ServiceLifetime.Transient : registry.attr.Lifetime == Lifetime.Scoped ? ServiceLifetime.Scoped : ServiceLifetime.Singleton)));
                directory.Add(contract, registry.service, registry.attr);
            }
            services.Add(new ServiceDescriptor(typeof(IPlugableServiceRegistry), directory));
        }

        private IEnumerable<(Type service, PlugableServiceAttribute[] attrs, Type[] interfaces)> FindAllPlugableServices(Assembly[] assemblies)
        {
            return assemblies.SelectMany(x => x.GetTypes())
                .Where(x => !x.IsAbstract && x.HasAttribute<PlugableServiceAttribute>()).Distinct()
                .Select(x => (
                service: x,
                attrs: x.GetCustomAttributes<PlugableServiceAttribute>().ToArray(),
                interfaces: x.GetCustomAttributes<PlugableServiceAttribute>().Select(a => a.ImplementationOf).ToArray()));
        }
    }
}