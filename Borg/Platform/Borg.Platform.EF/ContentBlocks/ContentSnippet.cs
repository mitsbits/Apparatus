using Borg.Infrastructure.Core.DDD.Contracts;
using Borg.Platform.EF.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using static Borg.Platform.EF.ContentBlocks.ContentSnippet;

namespace Borg.Platform.EF.ContentBlocks
{
    public class ContentSnippet : SiloedActivatable, IHaveKey<string>, IHaveValue<string>, IHaveTitle
    {
        public string Title { get; protected set; } = string.Empty;
        public string Value { get; protected set; } = string.Empty;
        public string Key { get; protected set; } = string.Empty;
        public ContentSnippetFlavour Flavour { get; protected set; } = ContentSnippetFlavour.Text;

        public enum ContentSnippetFlavour
        {
            Text,
            Html
        }
    }

    public class ContentSnippetInstruction<TDbContext> : SiloedActivatablenstruction<ContentSnippet, TDbContext> where TDbContext : DbContext
    {
        public override void ConfigureDb(ModelBuilder builder)
        {
            base.ConfigureDb(builder);
        }

        public override void ConfigureEntity(EntityTypeBuilder<ContentSnippet> builder)
        {
            base.ConfigureEntity(builder);
            builder.Property(x => x.Title).HasMaxLength(512).IsUnicode().IsRequired();
            builder.Property(x => x.Key).HasMaxLength(400).IsUnicode().IsRequired();
            builder.Property(x => x.Value).HasMaxLength(int.MaxValue).IsUnicode().IsRequired(false);

            var converter = new EnumToStringConverter<ContentSnippetFlavour>();
            builder.Property(x => x.Flavour).HasConversion(converter);
        }
    }
}