using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Apparatus.System.Backoffice;
using Borg.Framework.EF.Discovery;
using Borg.Framework.Reflection.Services;
using Borg.Infrastructure.Core.Reflection.Discovery;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace Apparatus.Application.Server
{
    public class Startup
    {

        private readonly ILoggerFactory loggerFactory;
        private readonly IWebHostEnvironment hostingEnvironment;
        private readonly IConfiguration configuration;
        private AssemblyExplorerResult entitiesExplorerResult;

        public Startup( IWebHostEnvironment hostingEnvironment, IConfiguration configuration)
        {
            this.loggerFactory = null;
            this.hostingEnvironment = hostingEnvironment;
            this.configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var b = new WebHostBuilder();
            var depAsmblPrv = new DepedencyAssemblyProvider(loggerFactory);
            var refAsmblPrv = new ReferenceAssemblyProvider(loggerFactory, null, GetType().Assembly);
            var explorer = new EntitiesExplorer(loggerFactory,
                    new IAssemblyProvider[]
                    {
                                refAsmblPrv,
                                depAsmblPrv
                    });

            entitiesExplorerResult = new AssemblyExplorerResult(loggerFactory, new[] { explorer });
            services.AddSingleton<IAssemblyExplorerResult>(entitiesExplorerResult);
            services.ConfigureOptions(typeof(UiConfigureOptions));
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {


                endpoints.MapControllers();

                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
            });
        }
    }
}
