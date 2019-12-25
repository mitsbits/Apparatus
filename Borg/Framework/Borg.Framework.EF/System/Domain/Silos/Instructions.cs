using Borg.Platform.EF.Instructions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Borg.Framework.EF.System.Domain.Silos
{
    public class SiloedInstruction<T, TDbContext> : EntityMap<T, TDbContext> where T : Siloed where TDbContext : DbContext
    {
        public override void ConfigureDb(ModelBuilder builder)
        {
            base.ConfigureDb(builder);
            var seqName = GetSequenceName(nameof(Siloed.Id));
            builder.HasSequence<int>(seqName);
        }

        public override void ConfigureEntity(EntityTypeBuilder<T> builder)
        {
            base.ConfigureEntity(builder);
            builder.HasKey(x => new { x.Id, x.TenantId, x.LanguageId });
            builder.HasOne<Tenant>(x => x.Tenant);
            builder.HasOne<Language>(x => x.Language);
        }
    }

    public class SiloedActivatablenstruction<T, TDbContext> : SiloedInstruction<T, TDbContext> where T : SiloedActivatable where TDbContext : DbContext
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

    public class TreenodeInstruction<T, TDbContext> : SiloedInstruction<T, TDbContext> where T : Treenode where TDbContext : DbContext
    {
        public override void ConfigureDb(ModelBuilder builder)
        {
            base.ConfigureDb(builder);
        }

        public override void ConfigureEntity(EntityTypeBuilder<T> builder)
        {
            base.ConfigureEntity(builder);
            builder.Property(x => x.ParentId).IsRequired(false);
            builder.Property(x => x.Depth).IsRequired().HasDefaultValue(0);
            builder.Property(x => x.Hierarchy).HasMaxLength(512).IsUnicode(false).IsRequired().HasDefaultValue(string.Empty);
        }
    }

    public class TreenodeActivatableInstruction<T, TDbContext> : SiloedActivatablenstruction<T, TDbContext> where T : TreenodeActivatable where TDbContext : DbContext
    {
        public override void ConfigureDb(ModelBuilder builder)
        {
            base.ConfigureDb(builder);
        }

        public override void ConfigureEntity(EntityTypeBuilder<T> builder)
        {
            base.ConfigureEntity(builder);
            builder.Property(x => x.ParentId).IsRequired(false);
            builder.Property(x => x.Depth).IsRequired().HasDefaultValue(0);
            builder.Property(x => x.Hierarchy).HasMaxLength(512).IsUnicode(false).IsRequired().HasDefaultValue(string.Empty);
        }
    }

    public class SlugTreenodeActivatableInstruction<T, TDbContext> : TreenodeActivatableInstruction<T, TDbContext> where T : SlugTreenodeActivatable where TDbContext : DbContext
    {
        public override void ConfigureDb(ModelBuilder builder)
        {
            base.ConfigureDb(builder);
        }

        public override void ConfigureEntity(EntityTypeBuilder<T> builder)
        {
            base.ConfigureEntity(builder);
            builder.Property(x => x.Slug).IsUnicode(true).HasMaxLength(1024).IsRequired(true).HasDefaultValue(string.Empty);
            builder.Property(x => x.FullSlug).IsUnicode(true).HasMaxLength(1024).IsRequired(true).HasDefaultValue(string.Empty);
        }
    }
}