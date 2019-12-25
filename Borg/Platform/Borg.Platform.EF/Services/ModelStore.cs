using Borg.Framework.Cms.Contracts;
using Borg.Framework.EF;
using Borg.Infrastructure.Core.DDD.Contracts;
using Borg.Infrastructure.Core.DDD.ValueObjects;
using Borg.Infrastructure.Core.Services.Factory;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Borg.Platform.EF.Services
{
    public class ModelStore<TEntity, TDb> : IModelStore<TEntity> where TEntity : class, IDataState, IIdentifiable where TDb : BorgDbContext
    {
        private readonly TDb _db;

        public ModelStore(TDb db)
        {
            _db = db;
        }

        public async Task<TEntity> Create(TEntity model)
        {
            _db.Set<TEntity>().Add(model);
            await _db.SaveChangesAsync();
            return model;
        }

        public async Task Delete(params CompositeKey[] keys)
        {
            var results = new List<TEntity>();
            foreach (var key in keys)
            {
                var predicate = await key.ToPredicate<TEntity>().AnyContext();
                var hit = _db.Set<TEntity>().FirstOrDefault(predicate);
                if (hit != null)
                {
                    results.Add(hit);
                }
            }
            if (results.Any())
            {
                foreach (var hit in results)
                {
                    _db.Remove<TEntity>(hit);
                }
                await _db.SaveChangesAsync().AnyContext();
            }
        }

        public Task<TEntity> Instance()
        {
            return Task.FromResult(New<TEntity>.Instance());
        }

        public async Task<TEntity> Item(CompositeKey key)
        {
            var predicate = await key.ToPredicate<TEntity>().AnyContext();
            return _db.Set<TEntity>().FirstOrDefault(predicate);
        }

        public async Task<IEnumerable<TEntity>> Items(int page, int rowCount)
        {
            return await _db.Set<TEntity>().Skip((page - 1) * rowCount).Take(rowCount).ToListAsync().AnyContext();
        }

        public async Task<TEntity> Update(TEntity model)
        {
            _db.Update<TEntity>(model);
            await _db.SaveChangesAsync();
            return model;
        }
    }
}