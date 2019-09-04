using Borg.Infrastructure.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;

namespace Borg.Framework.Services.Configuration
{
    public class Configurator<TConfiguration> : IDisposable

    {
        private readonly ILogger logger;
        private TConfiguration config;

        private Configurator(ILogger logger, IConfiguration configuration, Func<IConfiguration, IConfigurationSection> sectionProvider)
        {
            this.logger = logger;

            var section = sectionProvider.Invoke(configuration);
            Preconditions.NotNull(section, nameof(section));
            config = section.Get<TConfiguration>();
        }

        private Configurator(ILogger logger, IConfiguration configuration, string sectionName) : this(logger, configuration, (c) => c.GetSection(sectionName))
        {
        }

        public void Dispose()
        {
        }

        private TConfiguration Build()
        {
            return config;
        }

        public static TConfiguration Build(ILogger logger, IConfiguration configuration, Func<IConfiguration, IConfigurationSection> sectionProvider)
        {
            using (var configurator = new Configurator<TConfiguration>(logger, configuration, sectionProvider))
            {
                return configurator.Build();
            }
        }

        public static TConfiguration Build(ILogger logger, IConfiguration configuration, string sectionName)
        {
            using (var configurator = new Configurator<TConfiguration>(logger, configuration, sectionName))
            {
                return configurator.Build();
            }
        }
    }
}