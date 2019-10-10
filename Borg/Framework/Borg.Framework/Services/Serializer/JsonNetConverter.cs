using Borg.Infrastructure.Core.DI;
using Borg.Infrastructure.Core.Strings.Services;
using System.Text.Json;

namespace Borg.Framework.Services.Serializer
{
    [PlugableService(ImplementationOf = typeof(IJsonConverter), Lifetime = Lifetime.Singleton, OneOfMany = true, Order = 0)]
    public class JsonNetConverter : IJsonConverter
    {
        private readonly JsonSerializerOptions _settings;

        public JsonNetConverter(JsonSerializerOptions settings = null)
        {
            _settings = settings == null ? new JsonSerializerOptions() : settings;
        }

        public T DeSerialize<T>(string source)
        {
            return JsonSerializer.Deserialize<T>(source, _settings);
        }

        public string Serialize(object ob)
        {
            return JsonSerializer.Serialize(ob, _settings);
        }
    }
}