using Borg.Infrastructure.Core;
using System.Collections.Generic;
using System.Linq;

namespace Borg.Framework.SQLServer.Broadcast
{
    public class SubscribeCommand
    {
        public SubscribeCommand(string subscriberName, IEnumerable<string> queueNames)
        {
            SubscriberName = Preconditions.NotEmpty(subscriberName, nameof(subscriberName));
            QueueNames = Preconditions.NotEmpty(queueNames, nameof(queueNames)).ToArray();
        }

        public string SubscriberName { get; }
        public string[] QueueNames { get; }
    }
}