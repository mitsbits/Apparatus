using Autofac;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Borg.Platform.Dispatch.Autofac
{
    public class ServiceFactory : IDisposable
    {
        private readonly ILogger logger;
        private ILifetimeScope _scope;

        public ServiceFactory(IContainer container)
        {
            container = Preconditions.NotNull(container, nameof(container));
            _scope = container.BeginLifetimeScope();
            var loggerFactory = _scope.Resolve<ILoggerFactory>();
            logger = loggerFactory == null ? NullLogger.Instance : loggerFactory.CreateLogger(GetType());
        }

        public T GetInstance<T>()
        {
            using (var scope = _scope.BeginLifetimeScope())
            {
                logger.Trace($"{nameof(ServiceFactory)} requests service:{typeof(T)}");
                var service = scope.Resolve<T>();
                if (service != null)
                {
                    return service;
                }
                else
                {
                    logger.Trace($"{nameof(ServiceFactory)} failed to find service:{typeof(T)}");
                    return default;
                }
            }
        }

        public object GetInstance(Type type)
        {
            using (var scope = _scope.BeginLifetimeScope())
            {
                logger.Trace($"{nameof(ServiceFactory)} requests service:{type}");
                var service = Convert.ChangeType(scope.Resolve(type), type);
                if (service != null)
                {
                    return service;
                }
                else
                {
                    logger.Trace($"{nameof(ServiceFactory)} failed to find service:{type} ");
                    return default;
                }
            }
        }

        public IEnumerable<T> GetInstances<T>()
        {
            using (var scope = _scope.BeginLifetimeScope())
            {
                logger.Trace($"{nameof(ServiceFactory)} requests services:{typeof(T)}");
                var services = scope.Resolve<IEnumerable<T>>();
                if (services != null)
                {
                    return services;
                }
                else
                {
                    logger.Trace($"{nameof(ServiceFactory)} failed to find services:{typeof(T)}");
                    return default;
                }
            }
        }

        public IEnumerable GetInstances(Type type)
        {
            using (var scope = _scope.BeginLifetimeScope())
            {
                logger.Trace($"{nameof(ServiceFactory)} requests services:{type}");
                var enumerabletype = typeof(IEnumerable<>).MakeGenericType(new Type[] { type });
                var services = scope.Resolve(enumerabletype) as IEnumerable;
                if (services != null)
                {
                    return services;
                }
                else
                {
                    logger.Trace($"{nameof(ServiceFactory)} failed to find services:{type} ");
                    return default;
                }
            }
        }

        #region IDisposable Support

        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _scope.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~ServiceFactory()
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

        #endregion IDisposable Support
    }
}