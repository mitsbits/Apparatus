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

        private DateTimeOffset CreatedOn { get; set; }

        private string PayloadType { get; set; }
        private string Payload { get; set; }
    }
}