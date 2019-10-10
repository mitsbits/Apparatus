using Borg.Infrastructure.Core.DI;
using Borg.Infrastructure.Core.Services.Serializer;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Borg.Framework.Services.Serializer
{
    [PlugableService(ImplementationOf = typeof(ISerializer), Lifetime = Lifetime.Singleton, OneOfMany = true, Order = 9)]
    public class JsonNetSerializer : ISerializer
    {
        private readonly JsonSerializerOptions _settings;

        public JsonNetSerializer(JsonSerializerOptions settings = null)
        {
            _settings = settings == null ? new JsonSerializerOptions() : settings;
        }

        public Task<object> Deserialize(byte[] value)
        {
            return Task.FromResult(JsonSerializer.Deserialize<object>(Encoding.UTF8.GetString(value),options: _settings));
        }

        public Task<byte[]> Serialize(object value)
        {
            return value == null
                ? Task.FromResult<byte[]>(null)
                : Task.FromResult(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(value, _settings)));
        }
    }
}