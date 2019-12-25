using Borg.Infrastructure.Core;
using Borg.Infrastructure.Core.Exceptions;
using Borg.Platform.EF.Instructions.Contracts;
using Microsoft.EntityFrameworkCore;
using System;

namespace Borg.Framework.EF.Instructions
{
    public abstract class EntityMapBase : IEntityMap
    {
        protected EntityMapBase(Type entityType, Type contextType)
        {
            EntityType = Preconditions.NotNull(entityType, nameof(entityType));
            contextType = Preconditions.NotNull(contextType, nameof(contextType));
            if (!contextType.IsSubclassOf(typeof(DbContext)))
            {
                throw new NotSubclassOfException(contextType, typeof(DbContext));
            }
            ContextType = contextType;
        }

        public Type EntityType { get; }

        public Type ContextType { get; }

        public abstract void ConfigureDb(ModelBuilder builder);
        public abstract void Apply(ModelBuilder builder);

 
    }
}