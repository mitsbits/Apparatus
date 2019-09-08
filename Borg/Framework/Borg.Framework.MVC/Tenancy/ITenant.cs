using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Borg.Framework.MVC.Tenancy
{
    public interface ITenant
    {
        IHostingEnvironment HostingEnvironment { get; }
        IConfiguration Configuration { get; }
    }
}