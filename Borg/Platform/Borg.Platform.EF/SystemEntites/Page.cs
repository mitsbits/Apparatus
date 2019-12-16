using Borg.Framework.Cms.BuildingBlocks;
using Borg.Platform.EF.Silos;
using Microsoft.EntityFrameworkCore;

namespace Borg.Platform.EF.SystemEntites
{
    public class Page : TreenodeActivatable, IPage
    {
        public string Title { get; protected set; }

        public string Slug { get; protected set; }
    }

    public class PageInstruction : TreenodeActivatableInstruction<Page>
    {
        public override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Page>().Property(x => x.Title).IsUnicode(true).HasMaxLength(1024).IsRequired(true).HasDefaultValue(string.Empty);
            builder.Entity<Page>().Property(x => x.Slug).IsUnicode(true).HasMaxLength(1024).IsRequired(true).HasDefaultValue(string.Empty);
        }
    }
}