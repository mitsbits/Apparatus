using Borg.Framework.DAL;
using Borg.Infrastructure.Core;
using Borg.Infrastructure.Core.DDD.Contracts;
using Borg.Infrastructure.Core.DDD.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
namespace Borg.Framework.EF
{
    public abstract partial class BorgWithEventsDbContext : BaseBorgDbContext
    {
        protected event EventHandler<CollectionChangedEventArgs> CollectionChangedEventHandler;
        protected event EventHandler<IdentifiableChangedEventArgs> IdentifiableChangedEventHandler;

        protected ReportChangesEnebled ReportChanges { get; set; } = ReportChangesEnebled.All;

        private List<(Type type, EntityState state)> affectedCollections;
        private List<(Type type, EntityState state, CompositeKey key)> affectedIndentifiables;

        private ValueTask ThoseWhoAreAboutToCommit(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var affected = ChangeTracker.Entries().Where(x => x.State != EntityState.Unchanged);
            affectedCollections = new List<(Type type, EntityState state)>();
            affectedIndentifiables = new List<(Type type, EntityState state, CompositeKey key)>();
            foreach (var ent in affected)
            {
                var type = (type: ent.GetType(), state: ent.State);
                if (!affectedCollections.Contains(type)) affectedCollections.Add(type);
                var identifiable = ent as IIdentifiable;
                if (identifiable != null)
                {
                    affectedIndentifiables.Add((type: type.type, state: type.state, key: identifiable.Keys));
                }
            }
            return new ValueTask(Task.CompletedTask);
        }

        private ValueTask WorkOnCommited( CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            foreach (var collectionEvent in affectedCollections)
            {
                RaiseCollectionChanged(mapState(collectionEvent.state), collectionEvent.type);
            }

            foreach (var identifiableEvent in affectedIndentifiables)
            {
                RaiseIdentifiableChanged(mapState(identifiableEvent.state), identifiableEvent.type, identifiableEvent.key);
            }
            return new ValueTask(Task.CompletedTask);
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
            CollectionChangedEventHandler?.Invoke(this, new CollectionChangedEventArgs(operation, entityType));
        }

        private void RaiseIdentifiableChanged(CRUDOperation operation, Type entityType, CompositeKey keys)
        {
            IdentifiableChangedEventHandler?.Invoke(this, new IdentifiableChangedEventArgs(operation, entityType, keys));
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

        private static Func<EntityState, CRUDOperation> mapState = (s) =>
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


        protected enum ReportChangesEnebled
        {
            None = 0,
            All = 1
        }
    }


    public abstract class EntityStateChangedNotification
    {
        protected Func<EntityState, CRUDOperation> mapState = (s) =>
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

        protected Type EntityType { get; set; }
        public CRUDOperation Operation { get; protected set; }
    }

    public abstract class ChangedEventArgs : EventArgs
    {
        protected ChangedEventArgs(CRUDOperation operation, Type entityType) : base()
        {
            Operation = operation;
            EntityType = Preconditions.NotNull(entityType, nameof(entityType));
        }

        public CRUDOperation Operation { get; }
        public Type EntityType { get; }
    }

    public class CollectionChangedEventArgs : ChangedEventArgs
    {
        public CollectionChangedEventArgs(CRUDOperation operation, Type entityType) : base(operation, entityType)
        {
        }
    }

    public class IdentifiableChangedEventArgs : ChangedEventArgs
    {
        public IdentifiableChangedEventArgs(CRUDOperation operation, Type entityType, CompositeKey keys) : base(operation, entityType)
        {
            Keys = Preconditions.NotNull(keys, nameof(keys));
        }

        public CompositeKey Keys { get; }
    }

    public sealed class EntityTypeChanged : EntityStateChangedNotification
    {
        public EntityTypeChanged(Type type, EntityState state)
        {
            EntityType = Preconditions.NotNull(type, nameof(type));
            var localoperation = mapState(state);
            Operation = Preconditions.IsDefined<CRUDOperation>(localoperation, nameof(localoperation));
        }
    }

    public sealed class EntityChanged : EntityStateChangedNotification
    {
        public EntityChanged(Type type, EntityState state, CompositeKey keys)
        {
            EntityType = Preconditions.NotNull(type, nameof(type));
            var localoperation = mapState(state);
            Operation = Preconditions.IsDefined<CRUDOperation>(localoperation, nameof(localoperation));
            Keys = Preconditions.NotNull(keys, nameof(keys));
        }

        public CompositeKey Keys { get; }
    }


}
