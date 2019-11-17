using Borg.Framework.MVC.Features.HtmlPager;
using Microsoft.Extensions.Configuration;
using Borg;
namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigurePaginationSettings(this IServiceCollection services, IConfiguration configuration, string customSectionKey = "")
        {
            var key = customSectionKey.IsNullOrWhiteSpace() ? typeof(PaginationTagHelper).Namespace.Replace(".", ":") : customSectionKey;
            return services.Configure<PaginationConfiguration>(configuration.GetSection(key));
        }


    }
}