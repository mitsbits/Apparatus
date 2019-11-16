using Borg.Framework.ApplicationSettings;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationSettingTypeProvider(IServiceCollection services)
        {
            return services.AddSingleton<IApplicationSettingTypeProvider, ApplicationSettingTypeProvider>();
        }
    }
}