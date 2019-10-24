
using Microsoft.Extensions.Configuration;

namespace Borg.Framework.MVC.Tenancy
{
    public interface ITenant
    {

        IConfiguration Configuration { get; }
    }
}