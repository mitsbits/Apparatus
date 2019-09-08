﻿using Borg.Platform.Dispatch.Autofac;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDispatcherNetCore(this IServiceCollection services)
        {
            services.AddSingleton<ServiceFactory>();
            services.AddSingleton<IDispatcher, Dispatcher>();
            return services;
        }
    }
}