using Borg.Infrastructure.Core;
using Borg.Framework.Dispatch;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Text;
using System.Text.Json;

namespace Borg.Framework.SQLServer.ApplicationSettings
{
  public  class SqlSettingsConfigurationProvider : ConfigurationProvider
    {
        private readonly IMediator dispactcher;
        public SqlSettingsConfigurationProvider(IMediator dispatcher)
        {
            this.dispactcher = Preconditions.NotNull(dispatcher, nameof(dispatcher));
        }
        public override void Load()
        {
            var result = AsyncHelpers.RunSync(async()=> await this.dispactcher.Send(new GetSettingsCommand()));
            Data.Clear();
            foreach(var setting in result)
            {
                Data.Add(setting.PayloadType.FullName, JsonSerializer.Serialize(setting.Payload));
            }
        }

    }
}
