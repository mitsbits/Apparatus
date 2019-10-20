using System;
using System.Threading.Tasks;

namespace Borg.Framework.SQLServer.Broadcast
{
    public interface ISqlBroadcastBus
    {
        event EventHandler<QueueMessageArrivedEventArgs> OnMessageArrived;

        Task Publish(string queueName, object message);

        string SubcriberName { get; }
        string[] QueuesToListen { get; }
    }
}