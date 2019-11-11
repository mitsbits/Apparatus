using Borg.Infrastructure.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;

namespace Borg.Framework.Services.Configuration
{
    public class Configurator<TConfiguration> : IDisposable

    {

        private TConfiguration config;

        private Configurator(IConfiguration configuration, Func<IConfiguration, IConfigurationSection> sectionProvider)
        {
       

            var section = sectionProvider.Invoke(configuration);
            Preconditions.NotNull(section, nameof(section));
            config = section.Get<TConfiguration>();
        }

        private Configurator( IConfiguration configuration, string sectionName) : this( configuration, (c) => c.GetSection(sectionName))
        {
        }

        public void Dispose()
        {
        }

        private TConfiguration Build()
        {
            return config;
        }

        public static TConfiguration Build( IConfiguration configuration, Func<IConfiguration, IConfigurationSection> sectionProvider)
        {
            using (var configurator = new Configurator<TConfiguration>( configuration, sectionProvider))
            {
                return configurator.Build();
            }
        }

        public static TConfiguration Build( IConfiguration configuration, string sectionName)
        {
            using (var configurator = new Configurator<TConfiguration>( configuration, sectionName))
            {
                return configurator.Build();
            }
        }
    }
}