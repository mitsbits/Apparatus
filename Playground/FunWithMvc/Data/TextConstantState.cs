using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FunWithMvc.Data
{
    public class TextConstantState
    {

        public string Id { get; set; }
        public int TenantId { get; set; }
        public int LanguageId { get; set; }
        public string Value { get; set; }


    }

    internal class TextConstantStateTypeConfiguration : IEntityTypeConfiguration<TextConstantState>
    {
        public void Configure(EntityTypeBuilder<TextConstantState> builder)
        {
            builder.HasKey(x => new { x.TenantId, x.LanguageId, x.Id }).IsClustered();
            builder.Property(x => x.Id).HasMaxLength(500).IsUnicode();
            builder.Property(x => x.Value).HasMaxLength(int.MaxValue).IsUnicode();
        }
    }
}
