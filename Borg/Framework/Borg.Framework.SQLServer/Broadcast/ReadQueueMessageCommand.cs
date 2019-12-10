using Borg.Infrastructure.Core;
using Borg.Framework.Dispatch;

namespace Borg.Framework.SQLServer.Broadcast
{
    public class ReadQueueMessageCommand : IRequest<ReadQueueMessageCommandResult>
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