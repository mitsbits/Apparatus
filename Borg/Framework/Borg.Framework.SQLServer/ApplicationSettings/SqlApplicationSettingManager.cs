//using Borg.Framework.ApplicationSettings;
//using Borg.Infrastructure.Core;
//using MediatR;
//using Microsoft.Extensions.Logging;
//using Microsoft.Extensions.Logging.Abstractions;
//using System.Threading.Tasks;

//namespace Borg.Framework.SQLServer.ApplicationSettings
//{
//    public class SqlApplicationSettingManager<T> : IApplicationSettingsManager<T> where T : IApplicationSetting, new()
//    {

//        private readonly IMediator dispatcher;

//        public SqlApplicationSettingManager( IMediator dispatcher)
//        {
 
//            this.dispatcher = Preconditions.NotNull(dispatcher, nameof(dispatcher));
//        }

//        public async Task<T> Logg()
//        {
//            var command = new GetSettingCommand(typeof(T));
//            var result = await dispatcher.Send(command);
//            return (T)result.Payload;
//        }

//        public async Task<T> UpdateOrCreate(T updated)
//        {
//            var command = new UpdateOrCreateCommand(updated, typeof(T));
//            var result = await dispatcher.Send(command);
//            return (T)result.Payload;
//        }
//    }
//}