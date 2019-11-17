
using Borg.Framework.MVC.Options;
using Microsoft.AspNetCore.Hosting;

namespace Apparatus.System.Backoffice
{
    public sealed class UiConfigureOptions : ModuleUiPostConfigure
    {
        public UiConfigureOptions(IWebHostEnvironment environment) : base(environment)
        {
        }
    }
}