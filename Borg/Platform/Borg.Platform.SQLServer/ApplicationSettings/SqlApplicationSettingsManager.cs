using Borg.Framework.ApplicationSettings;
using Borg.Framework.SQLServer.ApplicationSettings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Borg.Platform.SQLServer.ApplicationSettings
{
    public class SqlApplicationSettingsManager : IApplicationSettingsManager
    {
        private readonly ILogger logger;
        public SqlApplicationSettingsManager(ILoggerFactory loggerFactory, IOptionsMonitor<SqlApplicationSettingConfiguration> options)
        {
            logger = loggerFactory.CreateForType(GetType());
        }
        public Task<T> Get<T>() where T : IApplicationSetting, new()
        {
            throw new NotImplementedException();
        }

        public Task<T> UpdateOrCreate<T>(T updated) where T : IApplicationSetting, new()
        {
            throw new NotImplementedException();
        }
    }
}
