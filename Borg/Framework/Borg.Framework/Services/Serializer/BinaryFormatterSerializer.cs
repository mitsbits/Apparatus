using Borg.Infrastructure.Core.DI;
using Borg.Infrastructure.Core.Services.Serializer;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Borg.Framework.Services.Serializer
{
    [PlugableService(ImplementationOf = typeof(ISerializer), Lifetime = Lifetime.Singleton, OneOfMany = true, Order = 99)]
    public class BinaryFormatterSerializer : ISerializer
    {
        public BinaryFormatterSerializer()
        {
        }

        public async Task<object> Deserialize(byte[] value, Type objectType)
        {
            if (objectType.Equals(typeof(string))) return await Deserialize(value);
            var formater = new BinaryFormatter();
            Stream stream = new MemoryStream(value);
            var result = formater.Deserialize(stream);
            return Task.FromResult(result);
        }

        public Task<string> Deserialize(byte[] data)
        {
            return Task.FromResult(Encoding.UTF8.GetString(data));
        }

        public async Task<byte[]> Serialize(object value)
        {
            if (value.GetType().Equals(typeof(string))) return await Serialize(value.ToString());
            byte[] result;
            using (var stream = new MemoryStream())
            {
                var formater = new BinaryFormatter();
                formater.Serialize(stream, value);
                result = stream.ToArray();
            }
            return result;
        }

        public Task<byte[]> Serialize(string value)
        {
            return Task.FromResult(Encoding.UTF8.GetBytes(value));
        }
    }
}