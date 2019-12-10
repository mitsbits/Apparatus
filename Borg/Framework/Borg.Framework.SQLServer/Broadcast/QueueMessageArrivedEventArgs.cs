using Borg.Infrastructure.Core;
using Borg.Framework.Dispatch;
using System;

namespace Borg.Framework.SQLServer.Broadcast
{
    public class QueueMessageArrivedEventArgs : EventArgs, INotification
    {
        public QueueMessageArrivedEventArgs(string queueName, string subcriberName, Type payloadType, object payload) : base()
        {
            QueueName = Preconditions.NotEmpty(queueName, nameof(queueName));
            SubcriberName = Preconditions.NotEmpty(subcriberName, nameof(subcriberName));
            PayloadType = Preconditions.NotNull(payloadType, nameof(payloadType));
            Payload = Preconditions.NotNull(payload, nameof(payload));
        }

        public string QueueName { get; }
        public string SubcriberName { get; }
        public object Payload { get; }
        public Type PayloadType { get; }
    }
}