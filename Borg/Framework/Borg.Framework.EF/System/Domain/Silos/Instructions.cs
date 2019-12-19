using Borg.Platform.EF.Instructions;
using Microsoft.EntityFrameworkCore;

namespace Borg.Framework.EF.System.Domain.Silos
{
    public class SiloedInstruction<T, TDbContext> : EntityMap<T, TDbContext> where T : Siloed where TDbContext : DbContext
    {
        public override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            var seqName = GetSequenceName(nameof(Siloed.Id));
            builder.HasSequence<int>(seqName);
            builder.Entity<T>().HasKey(x => new { x.Id, x.TenantId, x.LanguageId });
            builder.Entity<T>().HasOne<Tenant>(x => x.Tenant);
            builder.Entity<T>().HasOne<Language>(x => x.Language);
        }
    }

    public class SiloedActivatablenstruction<T, TDbContext> : SiloedInstruction<T, TDbContext> where T : SiloedActivatable where TDbContext : DbContext
    {
        public override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<T>().Property(x => x.ActiveFrom).IsRequired(false);
            builder.Entity<T>().Property(x => x.ActiveTo).IsRequired(false);
            builder.Entity<T>().Property(x => x.ActivationID).HasMaxLength(50).IsUnicode(false).IsRequired(false);
            builder.Entity<T>().Property(x => x.DeActivationID).HasMaxLength(50).IsUnicode(false).IsRequired(false);
            builder.Entity<T>().Property(x => x.IsActive).IsRequired();
            builder.Entity<T>().Property(x => x.IsCurrentlyActive).IsRequired();
        }
    }

    public class TreenodeInstruction<T, TDbContext> : SiloedInstruction<T, TDbContext> where T : Treenode where TDbContext : DbContext
    {
        public override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<T>().Property(x => x.ParentId).IsRequired(false);
            builder.Entity<T>().Property(x => x.Depth).IsRequired().HasDefaultValue(0);
            builder.Entity<T>().Property(x => x.Hierarchy).HasMaxLength(512).IsUnicode(false).IsRequired().HasDefaultValue(string.Empty);
        }
    }

    public class TreenodeActivatableInstruction<T, TDbContext> : SiloedActivatablenstruction<T, TDbContext> where T : TreenodeActivatable where TDbContext : DbContext
    {
        public override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<T>().Property(x => x.ParentId).IsRequired(false);
            builder.Entity<T>().Property(x => x.Depth).IsRequired().HasDefaultValue(0);
            builder.Entity<T>().Property(x => x.Hierarchy).HasMaxLength(512).IsUnicode(false).IsRequired().HasDefaultValue(string.Empty);
        }
    }
}