using Borg.Platform.EF.Instructions.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Borg
{
    public static class ModelBuilderExtensionMethods
    {
        public static void ApplyMap<TConfig>(this ModelBuilder builder, TConfig config) where TConfig : IEntityMap
        {
            if (builder == null) return;
            if (config == null) return;
            config.ConfigureDb(builder);
        }
    }
}