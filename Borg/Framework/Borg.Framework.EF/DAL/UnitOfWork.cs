using Borg.Framework.DAL;
using Borg.Framework.EF.Contracts;
using Borg.Infrastructure.Core;
using Borg.Platform.EF.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Borg.Framework.EF.DAL
{
    public class UnitOfWork<TDbContext> : IUnitOfWork<TDbContext> where TDbContext : BorgDbContextttt
    {
        protected ILogger Log;

        public UnitOfWork(ILoggerFactory loggerfactory, TDbContext dbContext)
        {
            Context = Preconditions.NotNull(dbContext, nameof(dbContext));
            Context.IsWrappedByUOW = true;
            Context.TrackedEventHandler += OnTracked;
            Context.StateChangedEventHandler += OnStateChanged;
            Log = loggerfactory == null ? NullLogger.Instance : loggerfactory.CreateLogger(GetType());
        }

        protected virtual TDbContext Context { get; }

        public IQueryRepository<T> QueryRepo<T>() where T : class
        {
            return Context.QueryRepo<T, TDbContext>();
        }

        public IReadWriteRepository<T> ReadWriteRepo<T>() where T : class
        {
            return Context.ReadWriteRepo<T, TDbContext>();
        }

        public Task<TEntity> Instance<TEntity>() where TEntity : class
        {
            if (!Context.EntityIsMapped<TEntity, TDbContext>()) throw new EntityNotMappedException<TDbContext>(typeof(TEntity));
            return Task.FromResult(Infrastructure.Core.Services.Factory.New<TEntity>.Instance.Invoke()); //TODO: this is not a nice namespace, move it to Borg
        }

        private void OnTracked(object sender, EntityTrackedEventArgs e)
        {
            var collection = new object[] { sender, e };
            var data = JsonSerializer.Serialize(collection);
            Log.LogTrace(data);
        }

        private void OnStateChanged(object sender, EntityStateChangedEventArgs e)
        {
            var collection = new object[] { sender, e };
            var data = JsonSerializer.Serialize(collection);
            Log.LogTrace(data);
        }

        public async Task Save(CancellationToken cancelationToken = default, bool supreseEvents = false)
        {
            try
            {
                EventHandler<EntityTrackedEventArgs> TrackedEventHandler = Context.TrackedEventHandler;
                EventHandler<EntityStateChangedEventArgs> StateChangedEventHandler = Context.StateChangedEventHandler;
                if (supreseEvents)
                {
                    Context.TrackedEventHandler -= OnTracked;
                    Context.StateChangedEventHandler -= OnStateChanged;
                }
                await Context.SaveChangesAsync(cancelationToken);
                if (supreseEvents)
                {
                    Context.TrackedEventHandler -= OnTracked;
                    Context.StateChangedEventHandler -= OnStateChanged;
                }

                Context.TrackedEventHandler += TrackedEventHandler;
                Context.StateChangedEventHandler += StateChangedEventHandler;
            }
            catch (DbUpdateConcurrencyException exception)
            {
                throw new ConcurrentModificationException(
                    "The record you attempted to edit was modified by another " +
                    "user after you loaded it. The edit operation was cancelled and the " +
                    "currect values in the database are displayed. Please try again.", exception);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                AsyncHelpers.RunSync(async () => await Save(CancellationToken.None, false));
                Context.TrackedEventHandler -= OnTracked;
                Context.StateChangedEventHandler -= OnStateChanged;
                Context.Dispose();
            }
        }
    }
}