using Borg.Infrastructure.Core;
using System;

namespace Borg.Framework.SQLServer.ApplicationSettings
{
    public class PaylodCommandResult
    {
        public PaylodCommandResult(Type payloadType, object payload)
        {
            PayloadType = Preconditions.NotNull(payloadType, nameof(payloadType));
            Payload = Preconditions.NotNull(payload, nameof(payload));
        }

        public Type PayloadType { get; }
        public object Payload { get; }
    }
}