using Borg.Framework.EF.Contracts;
using Borg.Infrastructure.Core.DDD.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Borg.Platform.EF.Instructions
{
    public abstract class EntityRegistry<TEntity> : IEntityRegistry where TEntity : IEntity
    {
        public abstract void RegisterWithDbContext(ModelBuilder builder);
    }
}