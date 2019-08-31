//using Borg.Framework.DAL;
//using Borg.Infrastructure.Core;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Logging;
//using System;

//namespace Borg.Framework.EF
//{
//    internal class Configurator<TConfiguration> : IDisposable where TConfiguration : BorgDbContextConfiguration
//    {
//        private readonly ILogger logger;
//        private TConfiguration config;
//        private readonly Confi

//        private Configurator(ILogger logger, IConfiguration configuration, Type dbType)
//        {
//            this.logger = logger;
//            var name = GetContextName(dbType);
//            IConfigurationSection section = configuration.GetSection(name);
//            if (section == null) section = configuration.GetSection(GetContextName(typeof(BorgDbContext)));
//            Preconditions.NotNull(section, nameof(section));
//            config = section.Get<TConfiguration>();
//            Preconditions.NotNull(config, nameof(config));
//        }

//        public void Dispose()
//        {
//            config = null;
//        }

//        private TConfiguration Build()
//        {
//            return config;
//        }

//        internal static TConfiguration Build(ILogger logger, IConfiguration configuration, Type dbType)
//        {
//            using (var configurator = new Configurator<TConfiguration>(logger, configuration, dbType))
//            {
//                return configurator.Build();
//            }
//        }

//        private string GetContextName(Type type)
//        {
//            Preconditions.NotNull(type, nameof(type));
//            var name = type.Name.Replace("Context", string.Empty).Slugify();
//            logger.Debug($"Resolving configuration {name} for {type.Name}");
//            if (!name.EndsWith("db")) name += "db";
//            return name;
//        }
//    }
//}