using Borg.Framework.Google.Protobuf;
using Borg.Infrastructure.Core.Services.Serializer;
using Microsoft.Extensions.DependencyInjection;
using ProtoBuf.Meta;
using System;
using System.Threading.Tasks;
using static Borg.Framework.Google.Protobuf.ProtobufSerializer;

namespace FunWithConsole
{
    class Program
    {
        private static IServiceProvider Locator;
        static async Task Main(string[] args)
        {
            var personMetaType = RuntimeTypeModel.Default.Add(typeof(Message), false);
            personMetaType.Add(1, "Recipients");
            personMetaType.Add(2, "Title");
        

            var services = new ServiceCollection();
            services.AddScoped<ISerializer, ProtobufSerializer>();
            services.AddLogging();
            Locator = services.BuildServiceProvider(true);


            var obj = new Message();

            var serializer =  Locator.CreateScope().ServiceProvider.GetRequiredService<ISerializer>();
            var data = await serializer.Serialize(obj);
            var outpput = await serializer.Deserialize(data);
        }
    }
}
