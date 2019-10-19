using System;
using System.Threading;
using System.Threading.Tasks;

namespace Borg.Infra.Messaging
{
    public interface IMessagePublisher
    {
        bool SupportsTopics { get; }

        Task PublishAsync(Type messageType, object message, TimeSpan? delay = null, CancellationToken cancellationToken = default);
    }

    public interface IQueuePublisher : IQueueClient
    {
        Task Publish(Type messageType, object message, CancellationToken cancellationToken = default);
    }
}