using Borg.Infrastructure.Core.DDD.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Borg.Platform.EF.Base
{
    public abstract class SlugTreenodeActivatable : TreenodeActivatable, IHaveSlug, IHaveFullSlug
    {
        public string Slug { get; protected set; }

        public string FullSlug { get; protected set; }
    }

    public abstract class SlugTreenodeActivatableInstruction<T, TDbContext> : TreenodeActivatableInstruction<T, TDbContext> where T : SlugTreenodeActivatable where TDbContext : DbContext
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