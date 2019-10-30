﻿using Borg.Framework.ApplicationSettings;
using Borg.Infrastructure.Core;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Threading.Tasks;

namespace Borg.Framework.SQLServer.ApplicationSettings
{
    public class SqlApplicationSettingManager<T> : IApplicationSettingManager<T> where T : IApplicationSetting, new()
    {
        private readonly ILogger logger;
        private readonly IMediator dispatcher;

        public SqlApplicationSettingManager(ILoggerFactory loggerFactory, IMediator dispatcher)
        {
            logger = loggerFactory == null ? NullLogger.Instance : loggerFactory.CreateLogger(GetType());
            this.dispatcher = Preconditions.NotNull(dispatcher, nameof(dispatcher));
        }

        public async Task<T> Get()
        {
            var command = new GetSettingCommand(typeof(T));
            var result = await dispatcher.Send(command);
            return (T)result.Payload;
        }

        public async Task<T> UpdateOrCreate(T updated)
        {
            var command = new UpdateOrCreateCommand(updated, typeof(T));
            var result = await dispatcher.Send(command);
            return (T)result.Payload;
        }
    }
}