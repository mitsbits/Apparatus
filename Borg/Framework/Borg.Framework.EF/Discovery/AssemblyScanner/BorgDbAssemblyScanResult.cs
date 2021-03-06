﻿using Borg.Infrastructure.Core;
using Borg.Infrastructure.Core.Reflection.Discovery;
using Borg.Platform.EF.Instructions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Borg.Framework.EF.Discovery.AssemblyScanner
{
    public class BorgDbAssemblyScanResult : AssemblyScanResult
    {
        private readonly List<Type> _dbs;
        private readonly List<Type> _states;
        private readonly List<Type> _maps;
        private readonly List<Type> _openMaps;

        public BorgDbAssemblyScanResult(Assembly assembly, IEnumerable<Type> dbs, IEnumerable<Type> states, IEnumerable<Type> maps, IEnumerable<Type> openMaps) : base(assembly, true, new string[0])
        {
            _dbs = new List<Type>();
            _states = new List<Type>();
            _maps = new List<Type>();
            _openMaps = new List<Type>();
            _dbs.AddRange(Preconditions.NotEmpty(dbs, nameof(dbs)));
            _states.AddRange(Preconditions.NotEmpty(states, nameof(states)));
            _maps.AddRange(Preconditions.NotEmpty(maps, nameof(maps)));
            _openMaps.AddRange(Preconditions.NotEmpty(openMaps, nameof(openMaps)));

        }

        public BorgDbAssemblyScanResult(Assembly assembly, string[] errors) : base(assembly, false, errors)
        {
            _dbs = null;
            _states = null;
            _maps = null;
            _openMaps = null;
        }

        public IEnumerable<Type> Dbs => _dbs;
        public IEnumerable<Type> DataStates => _states;
        public IEnumerable<Type> DefinedEntityMaps => _maps;
        public IEnumerable<Type> OpenEntityMaps => _openMaps;



        public IDictionary<Type, IEnumerable<Type>> DbEntities
        {
            get
            {
                //var db = _dbs.FirstOrDefault();
                //foreach (var openMap in _openMaps)
                //{
                //    _maps.Add(openMap.MakeGenericType(db));
                //}
                var generics = new List<Type>();
                foreach (var map in DefinedEntityMaps)
                {
                    generics.Add(map.GetBaseOpenGeneric(2));
                }



                var dict = generics.GroupBy(x => x.GenericTypeArguments[1]).ToDictionary(x => x.Key, x => x.Select(y => y.GenericTypeArguments[0]));

                return dict;
            }
        }
    }

    internal class BorgDbAssemblyScanner : AssemblyScanner<BorgDbAssemblyScanResult>, IDisposable
    {
        public BorgDbAssemblyScanner(Assembly assembly, ILoggerFactory loggerfactory) : base(loggerfactory, assembly)
        {
            Populate();
        }

        private BorgDbAssemblyScanResult Result;

        protected override Task<BorgDbAssemblyScanResult> ScanInternal()
        {
            return Task.FromResult(Result);
        }

        private void Populate()
        {
            var types = Assembly.GetTypes().ToList();
            var dbs = types.Where(t => t.IsBorgDb());
            var states = types.Where(t => t.IsDataState());
            var maps = types.Where(t => t.IsEntityMap());
            var openMaps = types.Where(t => t.IsOpenEntityMap());
            if (!dbs.Any() && !states.Any() && !maps.Any() && !openMaps.Any())
            {
                Result = new BorgDbAssemblyScanResult(Assembly, new string[] { new EmptyBorgDbScanResultException(Assembly).ToString() });
                return;
            }
            Result = new BorgDbAssemblyScanResult(Assembly, dbs, states, maps, openMaps);
        }

        #region IDisposable Support

        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~BorgDbAssemblyScanner()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }

        #endregion IDisposable Support
    }

    public class BorgDbAssemblyExplorer : AssemblyExplorer
    {
        private readonly List<AssemblyScanResult> results = new List<AssemblyScanResult>();
        private readonly ILoggerFactory loggerFactory;

        public BorgDbAssemblyExplorer(ILoggerFactory loggerfactory, IEnumerable<IAssemblyProvider> providers) : base(loggerfactory)
        {
            this.loggerFactory = loggerfactory;
            Populate(Preconditions.NotEmpty(providers, nameof(providers)));
            AsyncHelpers.RunSync(async () => await ScanInternal());
        }

        protected override IEnumerable<AssemblyScanResult> ResultsInternal()
        {
            return results;
        }

        protected override async Task ScanInternal()
        {
            foreach (var asml in assemblies)
            {
                results.Add(await ScanInternal(asml));
            }
            scanCompleted = true;
        }

        private async Task<AssemblyScanResult> ScanInternal(Assembly asmbl)
        {
            return await new AssemblyScanner.BorgDbAssemblyScanner(asmbl, loggerFactory).Scan();
        }

        private void Populate(IEnumerable<IAssemblyProvider> providers)
        {
            foreach (var asmbl in providers.SelectMany(p => p.GetAssemblies())
                .Where(asmbl => asmbl.Assimilated()
                    && !assemblies.Any(x => x.FullName == asmbl.FullName)))
            {
                Logger.Info($"Discoverd assembly for the hive - {asmbl.FullName}");
                assemblies.Add(asmbl);
            }
        }
    }
}