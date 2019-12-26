using Borg.Infrastructure.Core.DDD.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Borg.Platform.EF.Base
{
    public abstract class SiloedActivatable : Siloed, IActivatable
    {
        public DateTimeOffset? ActiveFrom { get; protected set; }

        public DateTimeOffset? ActiveTo { get; protected set; }

        public string ActivationID { get; protected set; } = string.Empty;

        public string DeActivationID { get; protected set; } = string.Empty;

        public bool IsActive { get; protected set; }
        public bool IsCurrentlyActive { get; protected set; }
    }

    public abstract class SiloedActivatablenstruction<T, TDbContext> : SiloedInstruction<T, TDbContext> where T : SiloedActivatable where TDbContext : DbContext
    {
        public override void ConfigureDb(ModelBuilder builder)
        {
            base.ConfigureDb(builder);
        }

        public override void ConfigureEntity(EntityTypeBuilder<T> builder)
        {
            base.ConfigureEntity(builder);
            builder.Property(x => x.ActivationID).HasMaxLength(50).IsUnicode(false).IsRequired(false);
            builder.Property(x => x.DeActivationID).HasMaxLength(50).IsUnicode(false).IsRequired(false);
            builder.Property(x => x.ActiveFrom).IsRequired(false);
            builder.Property(x => x.IsActive).IsRequired();
            builder.Property(x => x.IsCurrentlyActive).IsRequired();
            builder.Property(x => x.ActiveTo).IsRequired(false);
        }
    }
}