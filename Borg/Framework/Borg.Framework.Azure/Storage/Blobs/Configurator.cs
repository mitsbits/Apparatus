using Borg.Framework.DAL;
using Borg.Infrastructure.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;

namespace Borg.Framework.Azure.Storage.Blobs
{
    internal class AzureBlobConfigurator<TConfiguration> : IDisposable where TConfiguration : AzureBlobOptions
    {
        private readonly ILogger logger;
        private TConfiguration config;

        private AzureBlobConfigurator(ILogger logger, IConfiguration configuration, Type dbType)
        {
            this.logger = logger;
            var name = GetOptionsName(dbType);
            IConfigurationSection section = configuration.GetSection(name);
            if (section == null) section = configuration.GetSection(GetOptionsName(typeof(AzureBlobOptions)));
            Preconditions.NotNull(section, nameof(section));
            config = section.Get<TConfiguration>();
            Preconditions.NotNull(config, nameof(config));
        }

        public void Dispose()
        {
            config = null;
        }

        private TConfiguration Build()
        {
            return config;
        }

        internal static TConfiguration Build(ILogger logger, IConfiguration configuration, Type dbType)
        {
            using (var configurator = new AzureBlobConfigurator<TConfiguration>(logger, configuration, dbType))
            {
                return configurator.Build();
            }
        }

        private string GetOptionsName(Type type)
        {
            Preconditions.NotNull(type, nameof(type));
            var name = type.Name.Replace("Options", string.Empty).Slugify();
            logger.Debug($"Resolving configuration {name} for {type.Name}");
            if (!name.EndsWith("db")) name += "Options";
            return name;
        }
    }
}