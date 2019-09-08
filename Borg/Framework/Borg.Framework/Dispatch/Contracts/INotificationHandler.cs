using Borg.Infrastructure.Core.DDD.ValueObjects;
using System.Threading;
using System.Threading.Tasks;

namespace Borg.Framework.Dispatch.Contracts
{
    public interface INotificationHandler<in TNotification>
    {
        Task Handle(TNotification notification, CancellationToken cancellationToken);
    }

    public abstract class NotificationHandler
    {
        public abstract void Handle(object notification);
    }

    public abstract class AsyncNotificationHandler
    {
        public abstract Task Handle(object notification, CancellationToken cancellationToken);
    }

    public abstract class NotificationHandler<TNotification> : NotificationHandler, INotificationHandler<TNotification>
    {
        Task INotificationHandler<TNotification>.Handle(TNotification notification, CancellationToken cancellationToken)
        {
            Handle(notification);
            return Unit.Task;
        }
    }

    public abstract class AsyncNotificationHandler<TNotification> : AsyncNotificationHandler, INotificationHandler<TNotification>
    {
        Task INotificationHandler<TNotification>.Handle(TNotification notification, CancellationToken cancellationToken)
        {
            return Handle(notification, cancellationToken);
        }
    }
}