using Borg.Framework.Dispatch.Contracts;
using Borg.Framework.Services.Configuration;
using Borg.Infrastructure.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Threading.Tasks;

namespace Borg.Framework.SQLServer.Broadcast
{
    public interface ISqlBroadcastBus
    {
        Task Publish(string queueName, object message);

        string SubcriberName { get; }
        string[] QueuesToListen { get; }
    }

    public class SqlBroadcastBus : ISqlBroadcastBus
    {
        private readonly ILogger logger;
        private readonly IDispatcher dispatcher;

        public SqlBroadcastBus(ILoggerFactory loggerFactory, IDispatcher dispatcher, IConfiguration configuration)
        {
            logger = loggerFactory == null ? NullLogger.Instance : loggerFactory.CreateLogger(GetType());
            this.dispatcher = Preconditions.NotNull(dispatcher, nameof(dispatcher));
            configuration.GetSection(GetType().FullName.Replace(".", ":"));
            var options = Configurator<SqlBroadcastBusConfig>.Build(logger, configuration, GetType().FullName.Replace(".", ":"));
            SubcriberName = options.SubcriberName;
            QueuesToListen = options.QueuesToListen;
            AsyncHelpers.RunSync(() => dispatcher.Send(new SubscribeCommand(SubcriberName, QueuesToListen)));
        }

        public string SubcriberName { get; }

        public string[] QueuesToListen { get; }

        public async Task Publish(string queueName, object message)
        {
            var command = new AddQueueMessageCommand(queueName, message);
            await dispatcher.Publish(command);
        }
    }

    public class SqlBroadcastBusConfig
    {
        private string SqlConnectionString { get; set; }
        public string SubcriberName { get; set; }
        public string[] QueuesToListen { get; set; }
    }
}