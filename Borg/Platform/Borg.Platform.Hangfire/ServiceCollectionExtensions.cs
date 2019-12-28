using Borg.Framework.Services.Background;
using Borg.Platform.Hangfire;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        public static IServiceCollection AddHangfireSQL(this IServiceCollection services, IConfiguration configuration)
        {
            var hangfireconn = configuration["ConnectionStrings:HangfireConnection"];
            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                  .UseSqlServerStorage(hangfireconn, new SqlServerStorageOptions
                  {
                      CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                      SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                      QueuePollInterval = TimeSpan.Zero,
                      UseRecommendedIsolationLevel = true,
                      UsePageLocksOnDequeue = true,
                      DisableGlobalLocks = true
                  }));
            services.AddSingleton<IBackgroundRunner, HangfireBackgroundRunner>();
            return services;
        }
        public static IApplicationBuilder UseHangfireServices(this IApplicationBuilder app)
        {
            app.UseHangfireServer();
            app.UseHangfireDashboard(pathMatch: "/apparatus/hangfire");
            return app;
        }
    }


}
