using Borg.Framework.EF.Instructions;
using Borg.Framework.EF.Instructions.Attributes;

using Borg.Infrastructure.Core.Reflection.Discovery;
using Borg.Infrastructure.Core.Reflection.Discovery.ObjectGraph;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Borg.Framework.EF.Discovery.AssemblyScanner
{
    internal class EntitiesAssemblyScanner : AssemblyScanner<EntitiesAssemblyScanResult>, IDisposable
    {
        private List<Type> aggregateRoots;
        private Dictionary<Type, Type[]> complexTypes;
        private Type[] DefaultDbs;

        public EntitiesAssemblyScanner(Assembly assembly, ILoggerFactory loggerfactory) : base(loggerfactory, assembly)
        {
            aggregateRoots = new List<Type>();
            complexTypes = new Dictionary<Type, Type[]>();

            Populate();
        }

        protected override Task<EntitiesAssemblyScanResult> ScanInternal()
        {
            return Task.FromResult(Result);
        }

        private EntitiesAssemblyScanResult Result;

        private void Populate()
        {
            var aggregates = Assembly.GetTypes().Where(t => t.IsCmsAggregateRoot());
            if (!aggregates.Any())
            {
                Result = new EntitiesAssemblyScanResult(Assembly, new string[] { new NoAggregatesException(Assembly).ToString() });
                return;
            }

            aggregateRoots.AddRange(aggregates);

            foreach (var aggregate in aggregateRoots)
            {
                using (var recursor = new ComplexTypeRecursor(aggregate, null, Logger))
                {
                    try
                    {
                        var result = recursor.Results();
                        complexTypes.Add(aggregate, result.Entities());
                    }
                    catch (Exception ex)
                    {
                        Logger.Warn("{0}", ex.Message);
                    }
                }
            }

            var entityMaps = Assembly.GetTypes().Where(t => t.IsSubclassOfRawGeneric(typeof(GenericEntityMap<,>)) && !t.IsAbstract);
            foreach (var type in aggregates.Union(complexTypes.SelectMany(x => x.Value)).Distinct())
            {
                if (!entityMaps.Any(x => x.GetGenericArgumentType(0) == type))
                {
                    var attr = type.GetCustomAttributes().FirstOrDefault(x => typeof(EFAggregateRootAttribute).IsAssignableFrom(x.GetType()));
                    if (attr != null)
                    {
                        var dbType = ((EFAggregateRootAttribute)attr).DbType;
                    }
                }
            }
            var defalutDbs = Assembly.GetTypes().Where(x => x.GetCustomAttribute<DefaultDbDefinitionAttribute>() != null);
            Result = new EntitiesAssemblyScanResult(Assembly, aggregateRoots, complexTypes, defalutDbs, entityMaps.ToList());
        }

        public void Dispose()
        {
            aggregateRoots = null;
            complexTypes = null;
        }
    }
}