using Borg.Infrastructure.Core.DI;
using System;
using System.Collections.Generic;

namespace Borg.Framework.Reflection.ServiceRegistry
{
    [Obsolete]
    [PlugableService(ImplementationOf = typeof(IPlugableServiceRegistry), Lifetime = Lifetime.Singleton, OneOfMany = true, Order = 1)]
    public class PlugableServiceRegistry : IPlugableServiceRegistry
    {
        private readonly List<(Type contact, Type Service, PlugableServiceAttribute attribute)> _registry = new List<(Type contact, Type Service, PlugableServiceAttribute attribute)>();

        public IEnumerable<(Type contract, Type service, PlugableServiceAttribute attribute)> RegisteredServices()
        {
            return _registry;
        }

        internal void Add(Type contract, Type service, PlugableServiceAttribute attribute)
        {
            _registry.Add((contract, service, attribute));
        }
    }
}