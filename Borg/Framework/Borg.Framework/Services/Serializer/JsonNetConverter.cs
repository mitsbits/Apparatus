using Borg.Infrastructure.Core.DI;
using Borg.Infrastructure.Core.Strings.Services;
using Newtonsoft.Json;

namespace Borg.Framework.Services.Serializer
{
    [PlugableService(ImplementationOf = typeof(IJsonConverter), Lifetime = Lifetime.Singleton, OneOfMany = true, Order = 0)]
    public class JsonNetConverter : IJsonConverter
    {
        protected readonly JsonSerializerSettings _settings;

        public JsonNetConverter(JsonSerializerSettings settings = null)
        {
            _settings = settings ?? new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto };
        }

        public T DeSerialize<T>(string source)
        {
            return JsonConvert.DeserializeObject<T>(source, _settings);
        }

        public string Serialize(object ob)
        {
            return JsonConvert.SerializeObject(ob, _settings);
        }
    }
}