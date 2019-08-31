using Borg.Infrastructure.Core.DI;
using Borg.Infrastructure.Core.Services.Serializer;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Borg.Framework.Services.Serializer
{
    [PlugableService(ImplementationOf = typeof(ISerializer), Lifetime = Lifetime.Singleton, OneOfMany = true, Order = 9)]
    public class JsonNetSerializer : ISerializer
    {
        protected readonly JsonSerializerSettings _settings;

        public JsonNetSerializer(JsonSerializerSettings settings = null)
        {
            _settings = settings ?? new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto };
        }

        public Task<object> Deserialize(byte[] value, Type objectType)
        {
            return Task.FromResult(JsonConvert.DeserializeObject(Encoding.UTF8.GetString(value), objectType, _settings));
        }

        public Task<string> Deserialize(byte[] data)
        {
            return Task.FromResult(Encoding.UTF8.GetString(data));
        }

        public Task<byte[]> Serialize(object value)
        {
            return value == null
                ? Task.FromResult<byte[]>(null)
                : Task.FromResult(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value, _settings)));
        }

        public Task<byte[]> Serialize(string value)
        {
            return Task.FromResult(Encoding.UTF8.GetBytes(value));
        }
    }
}