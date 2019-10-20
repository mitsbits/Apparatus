using Borg.Framework.Caching;
using Borg.Framework.Modularity.Pipelines;
using Borg.Framework.Reflection.Services;
using Borg.Framework.Services.Serializer;
using Borg.Framework.SQLServer.Broadcast;
using Borg.Framework.SQLServer.Broadcast.Migration;
using Borg.Framework.Storage.Contracts;
using Borg.Framework.Storage.FileDepots;
using Borg.Infrastructure.Core.Services.Serializer;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Linq;

namespace TestFileProviders
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Environment = env;
        }

        public IConfiguration Configuration { get; }
        private IWebHostEnvironment Environment { get; }
        private ILoggerFactory LoggerFactory { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var asmmb = new DepedencyAssemblyProvider(LoggerFactory);
            services.AddMediatR(asmmb.GetAssemblies().ToArray());
            services.AddTransient<IHostStartUpJob, MigrationPipeline>();
            services.AddSingleton<ISqlBroadcastBus, SqlBroadcastBus>();
            var path = Path.Combine(Environment.WebRootPath, "static");
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            var memoryileProvider = new PhysicalFileDepot(Path.Combine(Environment.WebRootPath, "static"));
            services.AddSingleton<IFileProvider>(memoryileProvider);
            services.AddSingleton<IFileDepot>(memoryileProvider);
            services.AddControllersWithViews();
            services.AddDistributedMemoryCache();
            services.AddSingleton<ICacheClient, CacheClient>();
            services.AddSingleton<ISerializer, JsonNetSerializer>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}