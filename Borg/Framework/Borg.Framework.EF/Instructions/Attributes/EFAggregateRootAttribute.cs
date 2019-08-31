using Borg.Framework.Cms.Annotations;
using Borg.Infrastructure.Core;
using Borg.Infrastructure.Core.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;

namespace Borg.Framework.EF.Instructions.Attributes
{
    public class EFAggregateRootAttribute : CmsAggregateRootAttribute
    {
        private Type dbType;

        public EFAggregateRootAttribute(Type dbType)
        {
            var db = Preconditions.NotNull(dbType, nameof(dbType));
            if (!db.IsSubclassOf(typeof(DbContext)))
            {
                throw new NotSubclassOfException(dbType, typeof(DbContext));
            }
            else { dbType = db; }
        }

        public virtual Type DbType => dbType;
    }
}