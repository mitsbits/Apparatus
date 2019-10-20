using Borg;
using Borg.Framework.Modularity.Pipelines;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace TestFileProviders
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            Seed(host);
            host.Run();
        }

        public static void Seed(IHost host)
        {
            var jobs = host.Services.GetServices<IHostStartUpJob>();
            foreach (var job in jobs)
            {
                AsyncHelpers.RunSync(() => job.Execute());
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}