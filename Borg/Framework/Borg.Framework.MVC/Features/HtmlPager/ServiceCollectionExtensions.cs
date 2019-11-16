using Borg;
using Borg.Framework.MVC.Features.HtmlPager;
using Borg.Framework.Services.Configuration;
using Borg.Infrastructure.Core.Services.Factory;
using Microsoft.Extensions.Configuration;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        //public static IServiceCollection AddPagination(this IServiceCollection services)
        //{
        //    return services.AddSingleton<IPaginationSettingsProvider, NullPaginationSettingsProvider>();
        //}

        //public static IServiceCollection AddPagination<TSettings>(this IServiceCollection services, Func<TSettings> factory) where TSettings : IPaginationInfoStyle
        //{
          
        //    return services.AddSingleton<IPaginationSettingsProvider>(c => new FactoryPaginationSettingsProvider<TSettings>(factory));
        //}

        //public static IServiceCollection AddPagination<TSettings>(this IServiceCollection services, IConfigurationSection config) where TSettings : class, IPaginationInfoStyle, new()
        //{
        //    var settings = new TSettings();
        //    services.Config(config, () => settings);
        //    return services.AddSingleton<IPaginationSettingsProvider>(c => new InstancePaginationSettingsProvider<TSettings>(settings));
        //}

        public static IServiceCollection AddPagination<TSettings>(this IServiceCollection services, IConfiguration config) where TSettings : class, IPaginationInfoStyle, new()
        {
            var settings = Configurator<TSettings>.Build(config, typeof(PaginationTagHelper).Namespace.Replace(".", ":"));
            services.Config(config, () => settings);
            return services.AddSingleton<IPaginationSettingsProvider, InstancePaginationSettingsProvider<TSettings>>();
        }
    }
}