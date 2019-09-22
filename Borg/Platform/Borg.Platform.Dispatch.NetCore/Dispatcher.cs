using Borg.Framework.Dispatch.Contracts;
using Borg.Infrastructure.Core;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Threading;
using System.Threading.Tasks;

namespace Borg.Platform.Dispatch.NetCore
{
    public class Dispatcher : IDispatcher
    {
        private readonly ServiceFactory serviceFactory;
        private readonly ILogger logger;

        public Dispatcher(ServiceFactory serviceFactory)
        {
            this.serviceFactory = Preconditions.NotNull(serviceFactory, nameof(serviceFactory));
            var loggerFactory = serviceFactory.GetInstance<ILoggerFactory>();
            logger = loggerFactory == null ? NullLogger.Instance : loggerFactory.CreateLogger(GetType());
        }

        #region IDispatchePublishSender

        public Task Publish(object notification, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var notificationType = notification.GetType();
            var serviceType = typeof(INotificationHandler<>).MakeGenericType(notificationType);
            foreach (var handler in serviceFactory.GetInstances(serviceType))
            {
                var handlerType = handler.GetType();
                if (handlerType.IsSubclassOf(typeof(NotificationHandler)))
                {
                    logger.Trace($"Invoicing notification handler: {handlerType} for notification {notificationType}");
                    ((NotificationHandler)handler).Handle(notification);
                }

                if (handlerType.IsSubclassOf(typeof(AsyncNotificationHandler)))
                {
                    logger.Trace($"Invoicing notification handler: {handlerType} for notification {notificationType}");
                    ((AsyncNotificationHandler)handler).Handle(notification, cancellationToken);
                }
            }
            return Task.CompletedTask;
        }

        public Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var notificationType = notification.GetType();
            var serviceType = typeof(INotificationHandler<TNotification>);
            foreach (var handler in serviceFactory.GetInstances(serviceType))
            {
                var handlerType = handler.GetType();
                if (handlerType.IsSubclassOf(typeof(NotificationHandler)))
                {
                    logger.Trace($"Invoking notification handler: {handlerType} for notification {notificationType}");
                    ((NotificationHandler)handler).Handle(notification);
                }

                if (handlerType.IsSubclassOf(typeof(AsyncNotificationHandler)))
                {
                    logger.Trace($"Invoking notification handler: {handlerType} for notification {notificationType}");
                    ((AsyncNotificationHandler)handler).Handle(notification, cancellationToken);
                }
            }
            return Task.CompletedTask;
        }

        #endregion IDispatchePublishSender

        public async Task Send(object request, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var requestType = request.GetType();
            var serviceType = typeof(IRequestHandler<>).MakeGenericType(requestType);
            var handler = serviceFactory.GetInstance(serviceType);
            if (handler != null)
            {
                var handlerType = handler.GetType();
                if (handlerType.IsSubclassOfRawGeneric(typeof(RequestHandler)))
                {
                    logger.Trace($"Invoking request handler: {handlerType} for notification {requestType}");
                    handler.AsDynamic().Handle(request);
                }

                if (handlerType.IsSubclassOfRawGeneric(typeof(AsyncRequestHandler)))
                {
                    logger.Trace($"Invoking request handler: {handlerType} for notification {requestType}");
                    await handler.AsDynamic().Handle(request, cancellationToken);
                }
            }
        }

        public async Task<TResponse> Send<TRequest,TResponse >(TRequest request, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var requestType = request.GetType();
            var serviceType = typeof(IRequestHandler<,>).MakeGenericType(requestType, typeof(TResponse));
            var handler = serviceFactory.GetInstance(serviceType);
            if (handler != null)
            {
                var handlerType = handler.GetType();
                if (handlerType.IsSubclassOfRawGeneric(typeof(RequestHandler)))
                {
                    logger.Trace($"Invoking request handler: {handlerType} for notification {requestType}");
                    return (TResponse)(handler.AsDynamic()).Handle(request);
                }

                if (handlerType.IsSubclassOfRawGeneric(typeof(AsyncRequestHandler)))
                {
                    logger.Trace($"Invoking request handler: {handlerType} for notification {requestType}");
                    var result = await (handler.AsDynamic()).Handle(request, cancellationToken);
                    return (TResponse)result;
                }
            }
            return default;
        }
    }
}