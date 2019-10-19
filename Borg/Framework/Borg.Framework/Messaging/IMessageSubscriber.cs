using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Borg.Infra.Messaging
{
    public interface IMessageSubscriber
    {
        void Subscribe<T>(Func<T, CancellationToken, Task> handler,
            CancellationToken cancellationToken = default) where T : class;
    }

    public interface IQueueSubscriber<in T> : IQueueClient
    {
        string SubscriberName { get; }
        Func<T, CancellationToken, Task> Handler { get; }
    }

    public interface IQueueSubscriber : IQueueClient
    {
        string SubscriberName { get; }

        Task<object> Read();
    }

    public interface IJasonQueSubscriber : IQueueSubscriber<JsonDocument>
    {
    }

    public interface IQueueClient
    {
        string QueueName { get; }
    }
}