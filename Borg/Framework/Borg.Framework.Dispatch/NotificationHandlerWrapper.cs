using Borg.Framework.Dispatch.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Borg.Framework.Dispatch
{
    internal abstract class NotificationHandlerWrapper
    {
        public abstract Task Handle(object notification, CancellationToken cancellationToken, ServiceFactory serviceFactory, Func<IEnumerable<Func<Task>>, Task> publish);
    }

    internal class NotificationHandlerWrapperImpl<TNotification> : NotificationHandlerWrapper
    {
        public override Task Handle(object notification, CancellationToken cancellationToken, ServiceFactory serviceFactory, Func<IEnumerable<Func<Task>>, Task> publish)
        {
            var handlers = serviceFactory
                .GetInstances<INotificationHandler<TNotification>>()
                .Select(x => new Func<Task>(() => x.Handle((TNotification)notification, cancellationToken)));

            return publish(handlers);
        }
    }
}