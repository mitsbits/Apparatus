using Microsoft.EntityFrameworkCore;
using System;

namespace Borg.Platform.EF.Instructions.Contracts
{
    public interface IEntityMap
    {
        void OnModelCreating(ModelBuilder builder);

        Type EntityType { get; }
        Type ContextType { get; }
    }

    public interface IEntityMap<TEntity, TDbContext> : IEntityMap where TEntity : class where TDbContext : DbContext
    {
    }
}