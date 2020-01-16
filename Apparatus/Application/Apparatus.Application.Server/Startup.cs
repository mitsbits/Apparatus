using Apparatus.System.Backoffice.Areas.Apparatus;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Borg.Framework.EF;
using Borg.Framework.EF.Discovery;
using Borg.Framework.MVC.Features.EntityControllerFeature;
using Borg.Framework.Reflection.Services;
using Borg.Infrastructure.Core.Reflection.Discovery;
using Borg.Platform.EF;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Apparatus.Application.Server
{
    public class Startup
    {
        private readonly ILogger logger;
        private readonly IWebHostEnvironment hostingEnvironment;
        private readonly IConfiguration configuration;
        private AssemblyExplorerResult entitiesExplorerResult;

        public Startup(IWebHostEnvironment hostingEnvironment, IConfiguration configuration)
        {
            this.hostingEnvironment = hostingEnvironment;
            this.configuration = configuration;
        }
        public ILifetimeScope AutofacContainer { get; private set; }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var providers = new IAssemblyProvider[]
            {
              new DepedencyAssemblyProvider(null),
              new ReferenceAssemblyProvider(null, null, GetType().Assembly)
            };

            services.BorgDbAssemblyScan(providers, out var borgDbAssemblyScanResult);
            services.PlugableServicesAssemblyScan(providers, out var plugableServicesAssemblyScanResult);

            var explorer = new EntitiesExplorer(null, providers);

            entitiesExplorerResult = new AssemblyExplorerResult(null, new[] { explorer });
            services.AddSingleton<IAssemblyExplorerResult>(entitiesExplorerResult);

            services.Configure<KestrelServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

            // If using IIS:
            services.Configure<IISServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

            var config = configuration.GetSection("Borg:Platform:EF:Platform");
            var typed = config.Get<BorgDbContextConfiguration>();
            services.AddDbContext<PlatformDb>((o) =>
            {
                o.UseSqlServer(typed.ConnectionString, a =>
                {

                });

            });


            services.AddControllersWithViews().ConfigureApplicationPartManager(manager =>
            {
                manager.FeatureProviders.Add(new EntityControllerFeatureProvider(new[] { borgDbAssemblyScanResult, entitiesExplorerResult }));
            });
            services.Configure<RouteOptions>(routeOptions =>
            {
                routeOptions.ConstraintMap.Add("backofficeentitycontroller", typeof(BackOfficeEntityControllerConstraint));
                routeOptions.ConstraintMap.Add("backofficedbconextcontroller", typeof(BackOfficeDbContextControllerConstraint));
            });
            services.AddSession((o) => o.Cookie = new CookieBuilder() { IsEssential = true, Name = "apparatus_session", Path = "apparatus/" });


            services.AddHangfireSQL(configuration);


            services.AddServiceLocator();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {


            this.AutofacContainer = app.ApplicationServices.GetAutofacRoot();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHangfireServices();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {


                endpoints.MapDefaultControllerRoute();



            });

            app.MapWhen(c => c.Request.Path.Value.Contains("/apparatus", StringComparison.InvariantCultureIgnoreCase), app =>
            {
                if (env.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                }

                app.UseHangfireServices();
                app.UseStaticFiles();
                app.UseHsts();
                app.UseSession(new SessionOptions() { Cookie = new CookieBuilder() { IsEssential = true, Name = "apparatus_session", Path = "apparatus/" } });
                app.UseRouting();
                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapAreaControllerRoute(
                        name: "backofficeentity",
                        areaName: "apparatus",
                  pattern: "{area}/{dbcontext:backofficedbconextcontroller}/entity/{controller:backofficeentitycontroller}/{action=Index}/{id?}");

                    endpoints.MapAreaControllerRoute(
                        name: "apparatus",
                        areaName: "apparatus",
                        pattern: "{area}/{controller=Home}/{action=Index}/{id?}");
                    endpoints.MapControllers();
                });
            });
        }
    }
}