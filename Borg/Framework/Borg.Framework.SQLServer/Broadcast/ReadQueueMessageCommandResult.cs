using System;

namespace Borg.Framework.SQLServer.Broadcast
{
    public class ReadQueueMessageCommandResult
    {
        public ReadQueueMessageCommandResult()
        {
        }

        public ReadQueueMessageCommandResult(DateTimeOffset createdOn, string payloadType, string payload)
        {
            CreatedOn = createdOn;
            PayloadType = payloadType;
            Payload = payload;
        }

        public DateTimeOffset CreatedOn { get; set; }

        public string PayloadType { get; set; }
        public string Payload { get; set; }
    }
}