using Borg.Framework.EF.Instructions.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Borg
{
    public static class ModelBuilderExtensionMethods
    {
        public static void ApplyConfiguration<TConfig>(this ModelBuilder builder, TConfig config) where TConfig : IDbTypeConfiguration
        {
            if (builder == null) return;
            if (config == null) return;
            config.ConfigureDb(builder);
        }
    }
}