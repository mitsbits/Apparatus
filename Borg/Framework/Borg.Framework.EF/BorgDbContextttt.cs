using Borg.Framework.DAL;
using Borg.Framework.EF.Contracts;
using Borg.Framework.EF.Instructions;
using Borg.Framework.Services.Configuration;
using Borg.Infrastructure.Core;
using Borg.Infrastructure.Core.DDD.ValueObjects;
using Borg.Infrastructure.Core.Reflection.Discovery;
using Borg.Infrastructure.Core.Services.Factory;
using Borg.Platform.EF.Instructions;
using Borg.Platform.EF.Instructions.Contracts;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;

namespace Borg.Framework.EF
{
    public abstract class BorgDbContextttt<TConfiguration> : BorgDbContextttt where TConfiguration : IConfiguration
    {
        protected BorgDbContextttt(ILoggerFactory loggerFactory, TConfiguration configuration, IAssemblyExplorerResult explorerResult) : base(loggerFactory, configuration, explorerResult)
        {
        }

        protected BorgDbContextttt( DbContextOptions options) : base(options)
        {
        }

        protected BorgDbContextttt( DbContextOptions options, IAssemblyExplorerResult explorerResult) : base(options, explorerResult)
        {
        }
    }

    public abstract partial class BorgDbContextttt : DbContext
    {
        private IConfiguration configuration;

        public EventHandler<EntityTrackedEventArgs> TrackedEventHandler;
        public EventHandler<EntityStateChangedEventArgs> StateChangedEventHandler;

        protected readonly ILogger Logger;
        protected readonly IAssemblyExplorerResult ExplorerResult;

        private readonly SetUpMode Mode = SetUpMode.None;

        protected BorgDbContextttt(ILoggerFactory loggerFactory, IConfiguration configuration, IAssemblyExplorerResult explorerResult)
        {
            Logger = loggerFactory == null ? NullLogger.Instance : loggerFactory.CreateLogger(GetType());
            this.configuration = Preconditions.NotNull(configuration, nameof(configuration));
            this.ExplorerResult = Preconditions.NotNull(explorerResult, nameof(explorerResult));
            Mode = SetUpMode.Configuration;
        }

        protected BorgDbContextttt( DbContextOptions options) : base(options)
        {
        }

        protected BorgDbContextttt( DbContextOptions options, Func<BorgDbContextOptions> borgOptionsFactory = null) : base(options)
        {
        }

        protected BorgDbContextttt( DbContextOptions options, IAssemblyExplorerResult explorerResult) : base(options)
        {
            this.ExplorerResult = Preconditions.NotNull(explorerResult, nameof(explorerResult));
        }

        protected BorgDbContextttt( DbContextOptions options, BorgDbContextOptions borgOptions = null) : this(options, () => borgOptions)
        {
        }

        public virtual string Schema => (Mode == SetUpMode.Configuration) ? CheckOptionsForSchemaName() : GetType().Name.Replace("DbContext", string.Empty).Slugify();

        private BorgDbContextConfiguration BorgOptions { get; set; }
        public bool IsWrappedByUOW { get; set; } = false;

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            base.OnConfiguring(options);

            if (Mode == SetUpMode.Configuration)
            {
                SetUpConfig(options);
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            Map(builder);
        }

        protected enum SetUpMode
        {
            None,
            Configuration
        }

        public EventBehaviour EventBehaviour { get; set; } = EventBehaviour.AllowAll;

        #region Private

        private void Map(ModelBuilder builder)
        {
            MapEntities(builder);
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                entityType.SetSchema(GetContextName(GetType()));
            };
        }

        private void MapEntities(ModelBuilder builder)
        {
            var localresults = ExplorerResult.Results<EntitiesAssemblyScanResult>();
            foreach (var result in localresults)
            {
                if (!result.Success) continue;
                var entityTypes = result.AllEntityTypes();

                foreach (var entitytype in entityTypes)
                {
                    var isMapped = false;
                    var mapType = typeof(GenericEntityMap<,>).MakeGenericType(entitytype, GetType());
                    foreach (var mapdef in result.EntityMaps)
                    {
                        if (mapdef.IsAssignableTo(mapType))
                        {
                            if (isMapped) continue;
                            ((IEntityMap)New.Creator(mapdef)).OnModelCreating(builder);
                            isMapped = true;
                        }
                    }
                    if (!isMapped)
                    {
                        var newMapType = typeof(EntityMap<,>).MakeGenericType(entitytype, GetType());
                        ((IEntityMap)New.Creator(newMapType)).OnModelCreating(builder);
                        result.AddMap(newMapType);
                    }
                }
            }
        }

        private string CheckOptionsForSchemaName()
        {
            return (BorgOptions?.Overrides?.Schema ?? string.Empty).IsNullOrWhiteSpace()
              ? GetType().Name.Replace("DbContext", string.Empty).Slugify()
              : BorgOptions?.Overrides?.Schema;
        }

        private string GetContextName(Type type)
        {
            Preconditions.NotNull(type, nameof(type));
            var name = type.Name.Replace("Context", string.Empty).Slugify();
            if (!name.EndsWith("db")) name += "db";
            return name;
        }

        private void SetUpConfig(DbContextOptionsBuilder options)
        {
            BorgOptions = Configurator<BorgDbContextConfiguration>.Build(configuration, GetContextName(GetType()));
            options.UseSqlServer(BorgOptions.ConnectionString, opt =>
            {
                opt.EnableRetryOnFailure(3, TimeSpan.FromSeconds(30), new int[0]);
                opt.CommandTimeout(BorgOptions.Overrides.CommandTimeout);
            });
            options.EnableDetailedErrors(BorgOptions.Overrides.EnableDetailedErrors);
        }

        #endregion Private
    }

    public enum EventBehaviour
    {
        Supress,
        AllowAll
    }



}