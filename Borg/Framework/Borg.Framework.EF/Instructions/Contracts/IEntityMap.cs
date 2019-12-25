using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Borg.Platform.EF.Instructions.Contracts
{
    public interface IEntityMap
    {
        void ConfigureDb(ModelBuilder builder);

        Type EntityType { get; }
        Type ContextType { get; }
    }

    public interface IEntityMap<TEntity> : IEntityMap where TEntity : class
    {
        void ConfigureEntity(EntityTypeBuilder<TEntity> builder);
    }

    public interface IEntityMap<TEntity, TDbContext> : IEntityMap<TEntity> where TEntity : class where TDbContext : DbContext
    {
    }
}