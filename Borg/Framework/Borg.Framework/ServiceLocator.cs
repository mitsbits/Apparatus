using Borg.Infrastructure.Core;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Borg.Framework
{
    public class ServiceLocator : IDisposable
    {
        private IServiceProvider _currentServiceProvider;
        private static IServiceProvider _serviceProvider;

        public ServiceLocator(IServiceProvider currentServiceProvider)
        {
            _currentServiceProvider = Preconditions.NotNull(currentServiceProvider, nameof(currentServiceProvider));
        }

        public static ServiceLocator Current
        {
            get
            {
                return new ServiceLocator(_serviceProvider);
            }
        }

        public IServiceScope Scope()
        {
            return _currentServiceProvider.CreateScope();
        }

        public static void SetLocatorProvider(ServiceProvider serviceProvider)
        {
            _serviceProvider = Preconditions.NotNull(serviceProvider, nameof(serviceProvider));
        }

        #region Scoped

        public IServiceScope GetScopedInstance(Type serviceType, out object service)
        {
            var scope = Scope();
            service = GetInstance(serviceType, scope);
            return scope;
        }

        public IServiceScope GetScopedInstance<TService>(out TService service)
        {
            var scope = Scope();
            service = GetInstance<TService>(scope);
            return scope;
        }

        public IServiceScope GetScopedRequiredInstance(Type serviceType, out object service)
        {
            var scope = Scope();
            service = GetInstance(serviceType, scope, true);
            return scope;
        }

        public IServiceScope GetScopedRequiredInstance<TService>(out TService service)
        {
            var scope = Scope();
            service = GetInstance<TService>(scope, true);
            return scope;
        }

        public IServiceScope GetScopedInstances(Type serviceType, out IEnumerable services)
        {
            var scope = Scope();
            services = GetInstances(serviceType, scope);
            return scope;
        }

        public IServiceScope GetScopedInstances<TService>(out IEnumerable<TService> services)
        {
            var scope = Scope();
            services = GetInstances<TService>(scope).Cast<TService>();
            return scope;
        }

        #endregion Scoped

        #region ServiceProvider

        public object GetInstance(Type serviceType, IServiceScope scope = null, bool require = false)
        {
            serviceType = Preconditions.NotNull(serviceType, nameof(serviceType));
            return require
                ? (scope == null) ? _currentServiceProvider.GetRequiredService(serviceType) : scope.ServiceProvider.GetRequiredService(serviceType)
                : (scope == null) ? _currentServiceProvider.GetService(serviceType) : scope.ServiceProvider.GetService(serviceType);
        }

        public object GetRequiredInstance(Type serviceType)
        {
            return GetInstance(serviceType, null, true);
        }

        public object GetRequiredInstance<TService>(Type serviceType)
        {
            return GetInstance<TService>(null, true);
        }

        public TService GetInstance<TService>(IServiceScope scope = null, bool require = false)
        {
            return require
                ? (scope == null) ? _currentServiceProvider.GetRequiredService<TService>() : scope.ServiceProvider.GetRequiredService<TService>()
                : (scope == null) ? _currentServiceProvider.GetService<TService>() : scope.ServiceProvider.GetService<TService>();
        }

        public IEnumerable GetInstances(Type serviceType, IServiceScope scope = null)
        {
            return (scope == null) ? _currentServiceProvider.GetServices(serviceType) : scope.ServiceProvider.GetServices(serviceType);
        }

        public IEnumerable GetInstances<TService>(IServiceScope scope = null)
        {
            return (scope == null) ? _currentServiceProvider.GetServices<TService>() : scope.ServiceProvider.GetServices<TService>(); ;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                var dispasable = _currentServiceProvider as IDisposable;
                if (dispasable != null)
                {
                    dispasable.Dispose();
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion ServiceProvider
    }
}