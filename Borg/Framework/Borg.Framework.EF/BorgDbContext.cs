using Borg.Framework.DAL;
using Borg.Framework.Dispatch;
using Borg.Infrastructure.Core;
using Borg.Infrastructure.Core.DDD.Contracts;
using Borg.Infrastructure.Core.DDD.ValueObjects;
using Borg.Infrastructure.Core.Reflection.Discovery;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Borg.Framework.EF
{
    public abstract partial class BorgDbContext : BaseBorgDbContext
    {
        protected event EventHandler<CollectionChangedEventArgs>? CollectionChangedEventHandler;

        protected event EventHandler<IdentifiableChangedEventArgs>? IdentifiableChangedEventHandler;

        protected ReportChangesEnabled ReportChanges { get; set; } = ReportChangesEnabled.All;

        private List<(Type type, EntityState state)>? affectedCollections;
        private List<(Type type, EntityState state, IIdentifiable data)>? affectedIndentifiables;

        protected IMediator? dispatcher;

        protected BorgDbContext(bool suppressEvents) : base()
        {
            ReportChanges = ReportChangesEnabled.None;
        }

        protected BorgDbContext() : base()
        {
            WireUpContext();
        }

        protected BorgDbContext([NotNull] DbContextOptions options) : base(options)
        {
            WireUpContext();
        }

        protected BorgDbContext([NotNull] DbContextOptions options, [NotNull]IAssemblyExplorerResult explorerResult) : base(options, explorerResult)
        {
            WireUpContext();
        }

        private void WireUpContext()
        {
            PreSave.Add((a, c) => ThoseWhoAreAboutToCommit(c));
            //PreSave.Add((a, c) => TraverseTreeNodes(c));
            PostSave.Add((a, c) => RaiseEventsForAffectedEntities(c));
            PostSave.Add((a, c) => RaiseNotificationsForAffectedEntities(c));
        }

        //private Task SetAcivatebles(CancellationToken c)
        //{
        //   foreach(var ifd in affectedIndentifiables)
        //    {
        //       if (ifd.type.IsSubclassOf(typeof(SiloedActivatable)))
        //        {
        //            DetermineDelta(ifd.data as SiloedActivatable);
        //        }
        //    }
        //}

        //private void DetermineDelta(SiloedActivatable? siloedActivatable)
        //{
        //   var existingValues = db
        //}

        private Task RaiseNotificationsForAffectedEntities(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (ReportChanges == ReportChangesEnabled.None) return Task.CompletedTask;
            if (ReportChanges == ReportChangesEnabled.All || ReportChanges == ReportChangesEnabled.OnlyNotifications)
            {
                if (dispatcher == null) dispatcher = ServiceLocator.Current.GetInstance<IMediator>();
                var tasks = new List<Task>();

                tasks.AddRange(affectedCollections.Select(e =>
                    dispatcher.Publish<CollectionChangedEventArgs>(
                        new CollectionChangedEventArgs(mapState(e.state), e.type, GetType()))));

                tasks.AddRange(affectedIndentifiables.Select(e =>
                    dispatcher.Publish<IdentifiableChangedEventArgs>(
                        new IdentifiableChangedEventArgs(mapState(e.state), e.type, e.data.Keys, GetType()))));

                if (tasks.Count() == 0) return Task.CompletedTask;

                return Task.WhenAll(tasks);
            }
            return Task.CompletedTask;
        }

        private Task ThoseWhoAreAboutToCommit(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var affected = ChangeTracker.Entries().Where(x => x.State != EntityState.Unchanged);
            affectedCollections = new List<(Type type, EntityState state)>();
            affectedIndentifiables = new List<(Type type, EntityState state, IIdentifiable data)>();
            foreach (var ent in affected)
            {
                var type = (type: ent.GetType(), state: ent.State);
                if (!affectedCollections.Contains(type)) affectedCollections.Add(type);
                var identifiable = ent as IIdentifiable;
                if (identifiable != null)
                {
                    affectedIndentifiables.Add((type: type.type, state: type.state, data: identifiable));
                }
            }
            return Task.CompletedTask;
        }

        private Task RaiseEventsForAffectedEntities(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            foreach (var collectionEvent in affectedCollections ?? new List<(Type type, EntityState state)>())
            {
                RaiseCollectionChanged(mapState(collectionEvent.state), collectionEvent.type);
            }

            foreach (var identifiableEvent in affectedIndentifiables ?? new List<(Type type, EntityState state, IIdentifiable data)>())
            {
                RaiseIdentifiableChanged(mapState(identifiableEvent.state), identifiableEvent.type, identifiableEvent.data.Keys);
            }
            return Task.CompletedTask;
        }

        private void WorkOnCommited(List<(Type type, EntityState state)> affectedTypes, List<(Type type, EntityState state, CompositeKey key)> affectedIndentifiables)
        {
            foreach (var collectionEvent in affectedTypes)
            {
                RaiseCollectionChanged(mapState(collectionEvent.state), collectionEvent.type);
            }

            foreach (var identifiableEvent in affectedIndentifiables)
            {
                RaiseIdentifiableChanged(mapState(identifiableEvent.state), identifiableEvent.type, identifiableEvent.key);
            }
        }

        private void RaiseCollectionChanged(CRUDOperation operation, Type entityType)
        {
            if (ReportChanges == ReportChangesEnabled.None) return;
            if (ReportChanges == ReportChangesEnabled.All || ReportChanges == ReportChangesEnabled.OnlyEvents)
            {
                CollectionChangedEventHandler?.Invoke(this, new CollectionChangedEventArgs(operation, entityType, GetType()));
            }
        }

        private void RaiseIdentifiableChanged(CRUDOperation operation, Type entityType, CompositeKey keys)
        {
            if (ReportChanges == ReportChangesEnabled.None) return;
            if (ReportChanges == ReportChangesEnabled.All || ReportChanges == ReportChangesEnabled.OnlyEvents)
            {
                IdentifiableChangedEventHandler?.Invoke(this, new IdentifiableChangedEventArgs(operation, entityType, keys, GetType()));
            }
        }

        private void ThoseWhoAreAboutToCommit(out List<(Type type, EntityState state)> affectedTypes, out List<(Type type, EntityState state, CompositeKey key)> affectedIndentifiables)
        {
            var affected = ChangeTracker.Entries().Where(x => x.State != EntityState.Unchanged);
            affectedTypes = new List<(Type type, EntityState state)>();
            affectedIndentifiables = new List<(Type type, EntityState state, CompositeKey key)>();
            foreach (var ent in affected)
            {
                var type = (type: ent.GetType(), state: ent.State);
                if (!affectedTypes.Contains(type)) affectedTypes.Add(type);
                var identifiable = ent as IIdentifiable;
                if (identifiable != null)
                {
                    affectedIndentifiables.Add((type: type.type, state: type.state, key: identifiable.Keys));
                }
            }
        }

        private static readonly Func<EntityState, CRUDOperation> mapState = (s) =>
        {
            switch (s)
            {
                case EntityState.Modified:
                    return CRUDOperation.Update;
                    break;

                case EntityState.Added:
                    return CRUDOperation.Create;

                    break;

                case EntityState.Deleted:
                    return CRUDOperation.Delete;

                    break;

                case EntityState.Detached:
                    return CRUDOperation.NoAction;
                    break;

                case EntityState.Unchanged:
                    return CRUDOperation.NoAction;
                    break;
            }
            return CRUDOperation.NoAction;
        };

        public int SaveChanges(IMediator dispatcher)
        {
            this.dispatcher = Preconditions.NotNull(dispatcher, nameof(dispatcher));
            if (ReportChanges == ReportChangesEnabled.All || ReportChanges == ReportChangesEnabled.OnlyNotifications)
            {
                return base.SaveChanges();
            }
            throw new DbContextNotConfiguredForNotifications(ReportChanges);
        }

        public int SaveChanges(IMediator dispatcher, bool acceptAllChangesOnSuccess)
        {
            this.dispatcher = Preconditions.NotNull(dispatcher, nameof(dispatcher));
            if (ReportChanges == ReportChangesEnabled.All || ReportChanges == ReportChangesEnabled.OnlyNotifications)
            {
                return base.SaveChanges(acceptAllChangesOnSuccess);
            }
            throw new DbContextNotConfiguredForNotifications(ReportChanges);
        }

        public async Task<int> SaveChangesAsync(IMediator dispatcher, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            this.dispatcher = Preconditions.NotNull(dispatcher, nameof(dispatcher));
            if (ReportChanges == ReportChangesEnabled.All || ReportChanges == ReportChangesEnabled.OnlyNotifications)
            {
                return await base.SaveChangesAsync(cancellationToken);
            }
            return default;
        }

        public async Task<int> SaveChangesAsync(IMediator dispatcher, bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            this.dispatcher = Preconditions.NotNull(dispatcher, nameof(dispatcher));
            if (ReportChanges == ReportChangesEnabled.All || ReportChanges == ReportChangesEnabled.OnlyNotifications)
            {
                return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
            }
            throw new DbContextNotConfiguredForNotifications(ReportChanges);
        }

        public enum ReportChangesEnabled
        {
            None = 0,
            OnlyEvents = 1,
            OnlyNotifications = 2,
            All = 3
        }
    }

    public abstract class ChangedEventArgs : EventArgs
    {
        protected ChangedEventArgs(CRUDOperation operation, Type entityType, Type dbType) : base()
        {
            Operation = operation;
            EntityType = Preconditions.NotNull(entityType, nameof(entityType));
            DbType = Preconditions.NotNull(dbType, nameof(dbType));
        }

        public CRUDOperation Operation { get; }
        public Type EntityType { get; }
        public Type DbType { get; }
    }

    public class CollectionChangedEventArgs : ChangedEventArgs, INotification
    {
        public CollectionChangedEventArgs(CRUDOperation operation, Type entityType, Type dbType) : base(operation, entityType, dbType)
        {
        }
    }

    public class IdentifiableChangedEventArgs : ChangedEventArgs, INotification
    {
        public IdentifiableChangedEventArgs(CRUDOperation operation, Type entityType, CompositeKey keys, Type dbType) : base(operation, entityType, dbType)
        {
            Keys = Preconditions.NotNull(keys, nameof(keys));
        }

        public CompositeKey Keys { get; }
    }

    internal class DbContextNotConfiguredForNotifications : InvalidOperationException
    {
        public DbContextNotConfiguredForNotifications(BorgDbContext.ReportChangesEnabled reportChanges) : base(CreateExceptionMessage(reportChanges))
        {
        }

        private static string CreateExceptionMessage(BorgDbContext.ReportChangesEnabled reportChanges)
        {
            return $"{nameof(BorgDbContext)} has notification functionaliy disabled because of {nameof(BorgDbContext.ReportChangesEnabled)} setting has value {reportChanges.ToString()}";
        }
    }
}