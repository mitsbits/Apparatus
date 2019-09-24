using Borg.Framework;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Borg.Platform.Dispatch.NetCore
{
    public class ServiceFactory
    {
        private readonly ILogger logger;

        public ServiceFactory()
        {
            var loggerFactory = ServiceLocator.Current.GetInstance<ILoggerFactory>();
            logger = loggerFactory == null ? NullLogger.Instance : loggerFactory.CreateLogger(GetType());


        }

        public T GetInstance<T>()
        {
            using (var scope = ServiceLocator.Current.GetScopedRequiredInstance<T>(out var service))
            {
                logger.Trace($"{nameof(ServiceFactory)} requests service:{typeof(T)}");
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
            using (var scope = ServiceLocator.Current.GetScopedRequiredInstance(type, out var service))
            {
                logger.Trace($"{nameof(ServiceFactory)} requests service:{type}");
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
            using (var scope = ServiceLocator.Current.GetScopedInstances<T>(out var service))
            {
                logger.Trace($"{nameof(ServiceFactory)} requests services:{typeof(T)}");
                if (service != null)
                {
                    return service;
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
            using (var scope = ServiceLocator.Current.GetScopedInstances(type, out var service))
            {
                logger.Trace($"{nameof(ServiceFactory)} requests services:{type}");
                if (service != null)
                {
                    return service;
                }
                else
                {
                    logger.Trace($"{nameof(ServiceFactory)} failed to find services:{type} ");
                    return default;
                }
            }
        }
    }
}