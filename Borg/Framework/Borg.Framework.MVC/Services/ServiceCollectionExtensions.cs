using Borg.Framework.MVC.Services;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
   public static partial class ServiceCollectionExtensions
    {
        public static IServiceCollection AddViewToStringRendererService(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();          
            services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped<ViewToStringRendererService, ViewToStringRendererService>();
            return services;
        }
    }
}
