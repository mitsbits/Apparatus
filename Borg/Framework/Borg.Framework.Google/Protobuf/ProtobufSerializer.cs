using Borg.Infrastructure.Core.Services.Serializer;
using Microsoft.Extensions.Logging;
using ProtoBuf;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Borg.Framework.Google.Protobuf
{
    public class ProtobufSerializer : ISerializer
    {
        private readonly ILogger logger;

        public ProtobufSerializer(ILogger<ProtobufSerializer> logger)
        {   
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            logger.Trace($"{nameof(ProtobufSerializer)} is instanciating.");
        }
        public Task<object> Deserialize(byte[] data)
        {

            using (var stream = new System.IO.MemoryStream(data))
            {
                stream.Position = 0;
                var o = Serializer.Deserialize<Message>(stream);
                return Task.FromResult(o as object);
            }
        }

        public Task<byte[]> Serialize(object value)
        {
            using (var stream = new System.IO.MemoryStream())

            {
                Serializer.Serialize(stream, value);
                stream.Position = 0;
                return Task.FromResult(stream.ToArray());
            }
        }
      
        public class Message
        {
         
            public string[] Recipients { get; set; } = new[] { "Mitsos", "Kitsos", "Frixos" };
     
            public string Title { get; set; } = "The message";
        }
    }
}