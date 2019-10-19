using Borg.Infrastructure.Core;

namespace Borg.Framework.SQLServer.Broadcast
{
    public class AddQueueMessageCommand
    {
        public AddQueueMessageCommand(string queueName, object message)
        {
            Payload = Preconditions.NotNull(message, nameof(message));
            QueueName = Preconditions.NotEmpty(queueName, nameof(queueName));
        }

        public object Payload { get; }
        public string QueueName { get; }
    }
}