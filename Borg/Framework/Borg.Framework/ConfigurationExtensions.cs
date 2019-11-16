using Borg.Infrastructure.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Borg
{
    public static class ConfigurationExtensions
    {
        public static TConfig Config<TConfig>(this IServiceCollection services, IConfiguration configuration,
            Func<TConfig> pocoProvider) where TConfig : class
        {
            Preconditions.NotNull(services, nameof(services));
            Preconditions.NotNull(configuration, nameof(configuration));
            Preconditions.NotNull(pocoProvider, nameof(pocoProvider));

            var config = pocoProvider();
            configuration.Bind(config);
 
            services.AddOptions<TConfig>();

            return config;
        }

        public static TConfig Config<TConfig>(this IServiceCollection services, IConfiguration configuration,
            TConfig config) where TConfig : class
        {
            Preconditions.NotNull(services, nameof(services));
            Preconditions.NotNull(configuration, nameof(configuration));
            Preconditions.NotNull(config, nameof(config));

            configuration.Bind(config);
            services.AddOptions<TConfig>();

            return config;
        }
    }
}