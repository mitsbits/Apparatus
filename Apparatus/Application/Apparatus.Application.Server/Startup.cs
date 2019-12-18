using Borg.Framework.EF.Discovery;
using Borg.Framework.MVC.Features.EntityControllerFeature;
using Borg.Framework.Reflection.Services;
using Borg.Infrastructure.Core.Reflection.Discovery;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
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
        private readonly ILoggerFactory loggerFactory;
        private readonly IWebHostEnvironment hostingEnvironment;
        private readonly IConfiguration configuration;
        private AssemblyExplorerResult entitiesExplorerResult;

        public Startup(IWebHostEnvironment hostingEnvironment, IConfiguration configuration)
        {
            this.loggerFactory = null;
            this.hostingEnvironment = hostingEnvironment;
            this.configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //var b = new WebHostBuilder();
            var depAsmblPrv = new DepedencyAssemblyProvider(loggerFactory);
            var refAsmblPrv = new ReferenceAssemblyProvider(loggerFactory, null, GetType().Assembly);

            services.BorgDbAssemblyScan(new IAssemblyProvider[]
                    {
                                refAsmblPrv,
                                depAsmblPrv
                    });

            var explorer = new EntitiesExplorer(loggerFactory,
                    new IAssemblyProvider[]
                    {
                                refAsmblPrv,
                                depAsmblPrv
                    });

            entitiesExplorerResult = new AssemblyExplorerResult(loggerFactory, new[] { explorer });
            services.AddSingleton<IAssemblyExplorerResult>(entitiesExplorerResult);

            services.AddControllersWithViews().ConfigureApplicationPartManager(manager =>
            {
                manager.FeatureProviders.Add(new BackOfficeEntityControllerFeatureProvider(entitiesExplorerResult));
            });
            services.Configure<RouteOptions>(routeOptions =>
            {
                routeOptions.ConstraintMap.Add("backofficeentitycontroller", typeof(BackOfficeEntityControllerConstraint));
            });
            services.AddSession((o) => o.Cookie = new CookieBuilder() { IsEssential = true, Name = "apparatus_session", Path = "apparatus/" });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.MapWhen(c => c.Request.Path.Value.Contains("/apparatus", StringComparison.InvariantCultureIgnoreCase), app =>
            {
                app.UseHsts();
                app.UseSession(new SessionOptions() { Cookie = new CookieBuilder() { IsEssential = true, Name = "apparatus_session", Path = "apparatus/" } });
                app.UseRouting();
                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapAreaControllerRoute(
                        name: "backofficeentity",
                        areaName: "apparatus",
                        pattern: "apparatus/entity/{controller:backofficeentitycontroller}/{action=Index}/{id?}");

                    endpoints.MapAreaControllerRoute(
                        name: "apparatus",
                        areaName: "apparatus",
                        pattern: "apparatus/{controller=Home}/{action=Index}/{id?}");
                    endpoints.MapControllers();
                });
            });

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", context =>
                {
                    context.Response.Redirect("apparatus");
                    return Task.CompletedTask;
                });
            });
        }
    }
}