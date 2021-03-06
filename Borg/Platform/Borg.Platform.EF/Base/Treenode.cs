﻿using Borg.Infrastructure.Core.DDD.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Borg.Platform.EF.Base
{
    public abstract class Treenode : Siloed, ITreeNode
    {
        public int? ParentId { get; protected set; }

        public int Depth { get; protected set; }

        public string Hierarchy { get; protected set; }
    }

    public abstract class TreenodeInstruction<T, TDbContext> : SiloedInstruction<T, TDbContext> where T : Treenode where TDbContext : DbContext
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
}