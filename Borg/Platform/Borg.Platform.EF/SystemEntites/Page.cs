using Borg.Framework.Cms.BuildingBlocks;
using Borg.Framework.EF.System.Domain.Silos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Borg.Platform.EF.SystemEntites
{
    public class Page : SlugTreenodeActivatable, IPage
    {
        public string Title { get; protected set; }
    }

    public class PageInstruction : SlugTreenodeActivatableInstruction<Page, PlatformDb>
    {
        public override void ConfigureDb(ModelBuilder builder)
        {
            base.ConfigureDb(builder);
            
        }

        public override void ConfigureEntity(EntityTypeBuilder<Page> builder)
        {
            base.ConfigureEntity(builder);
            builder.Property(x => x.Title).IsUnicode(true).HasMaxLength(1024).IsRequired(true).HasDefaultValue(string.Empty);
        }
    }
}