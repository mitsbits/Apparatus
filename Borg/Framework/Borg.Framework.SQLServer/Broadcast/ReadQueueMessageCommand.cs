using Borg.Infrastructure.Core;

namespace Borg.Framework.SQLServer.Broadcast
{
    public class ReadQueueMessageCommand
    {
        public ReadQueueMessageCommand(string queueName, string subscriberName)
        {
            QueueName = Preconditions.NotEmpty(queueName, nameof(queueName));
            SubscriberName = Preconditions.NotEmpty(subscriberName, nameof(subscriberName));
        }

        public string QueueName { get; }
        public string SubscriberName { get; }
    }
}