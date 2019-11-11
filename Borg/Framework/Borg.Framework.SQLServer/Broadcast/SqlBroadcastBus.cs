using Borg.Framework.Services.Configuration;
using Borg.Infrastructure.Core;
using Borg.Infrastructure.Core.Threading;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Borg.Framework.SQLServer.Broadcast
{
    public class SqlBroadcastBus : ISqlBroadcastBus, IDisposable
    {
        private readonly ILogger logger;
        private readonly IMediator dispatcher;
        private readonly AsyncLock asyncLock;
        private ScheduledTimer _queueTimer;

        public SqlBroadcastBus(ILoggerFactory loggerFactory, IMediator dispatcher, IConfiguration configuration)
        {
            logger = loggerFactory == null ? NullLogger.Instance : loggerFactory.CreateLogger(GetType());
            this.dispatcher = Preconditions.NotNull(dispatcher, nameof(dispatcher));
            var options = Configurator<SqlBroadcastBusConfig>.Build( configuration, SqlBroadcastBusConfig.Key);
            SubcriberName = options.SubcriberName;
            QueuesToListen = options.QueuesToListen;
            PollingIntervalInSeconds = options.PollingIntervalInSeconds;
            AsyncHelpers.RunSync(() => dispatcher.Send(new SubscribeCommand(SubcriberName, QueuesToListen)));
            asyncLock = new AsyncLock();
            _queueTimer = new ScheduledTimer(DoListenToQueues, loggerFactory, null, TimeSpan.FromSeconds(PollingIntervalInSeconds));
        }

        public event EventHandler<QueueMessageArrivedEventArgs> OnMessageArrived;

        private async Task<DateTime?> DoListenToQueues()
        {
            foreach (var queue in QueuesToListen)
            {
                using (asyncLock.Lock())
                {
                    var command = new ReadQueueMessageCommand(queue, SubcriberName);
                    var result = await dispatcher.Send(command);
                    if (result != null)
                    {
                        var type = Type.GetType(result.PayloadType);
                        var payload = JsonSerializer.Deserialize(result.Payload, type);
                        var args = new QueueMessageArrivedEventArgs(queue, SubcriberName, type, payload);
                        await dispatcher.Publish(new QueueMessageArrivedEventArgs(queue, SubcriberName, type, payload));
                        OnMessageArrived?.Invoke(this, args);
                    }
                }
            }
            return DateTime.Now.Add(TimeSpan.FromSeconds(PollingIntervalInSeconds));
        }

        public string SubcriberName { get; }
        public int PollingIntervalInSeconds { get; }

        public string[] QueuesToListen { get; }

        public async Task Publish(string queueName, object message)
        {
            var command = new AddQueueMessageCommand(queueName, message);
            await dispatcher.Send(command);
        }

        public void Dispose()
        {
            _queueTimer?.Dispose();
        }
    }
}