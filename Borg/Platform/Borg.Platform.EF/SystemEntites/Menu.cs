using Borg.Framework.EF.System.Domain.Silos;
using Borg.Infrastructure.Core.DDD.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Collections.Generic;

namespace Borg.Platform.EF.SystemEntites
{
    public class Menu : SiloedActivatable, IHaveTitle
    {
        public virtual ICollection<MenuItem> Items { get; protected set; } = new HashSet<MenuItem>();

        public string Title { get; protected set; }
    }

    public class MenuItem : TreenodeActivatable
    {
        public virtual int MenuId { get; protected set; }
        public virtual Menu Menu { get; protected set; }
        public virtual Targets Targets { get; protected set; }
    }

    public enum Targets
    {
        _self,
        _blank,
        _partent,
        _top
    }

    public class MenuInstruction : SiloedActivatablenstruction<Menu, PlatformDb>
    {
        public override void ConfigureDb(ModelBuilder builder)
        {
            base.ConfigureDb(builder);

        
        }

        public override void ConfigureEntity(EntityTypeBuilder<Menu> builder)
        {
            base.ConfigureEntity(builder);
            builder.Property(x => x.Title).HasMaxLength(1024).IsRequired(true);
        }
    }

    public class MenuItemInstruction : TreenodeActivatableInstruction<MenuItem, PlatformDb>
    {
        public override void ConfigureDb(ModelBuilder builder)
        {
            base.ConfigureDb(builder);


        }

        public override void ConfigureEntity(EntityTypeBuilder<MenuItem> builder)
        {
            base.ConfigureEntity(builder);
            builder.Property(e => e.Targets).HasConversion(new EnumToStringConverter<Targets>());
            builder.Property(x => x.MenuId).IsRequired(true);
            builder.HasIndex(x => x.MenuId).HasName("FK_Menu_Id");
            builder.HasOne(x => x.Menu).WithMany(x => x.Items).HasForeignKey(x => x.MenuId);
        }
    }
}