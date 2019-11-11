using Borg.Framework.Services.Configuration;
using Borg.Framework.SQLServer.ApplicationSettings;
using Borg.Platform.SQLServer.ApplicationSettings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Borg.Platform.SQLServer
{
    public static partial class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSqlApplicationSettings(this IServiceCollection services, IConfiguration configuration)
        {
            var key = typeof(SqlApplicationSettingsManager).FullName.Replace(".", ":");
            var config = Configurator<SqlApplicationSettingConfiguration>.Build(configuration, key);
            configuration.GetSection(key).Bind(config);
            services.ConfigureOptions<SqlApplicationSettingConfiguration>();
            return services;
        }
    }
}
