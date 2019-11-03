using Borg.Framework.MVC.Features.Scripts;
using Microsoft.Extensions.DependencyInjection;

namespace Borg
{
    public static partial class DependencyInjectionExtensions
    {
        public static IServiceCollection AddScriptStore(this IServiceCollection services, bool asSinglenton = false)
        {
            if (asSinglenton)
            {
                return services.AddSingleton<IScriptStore, ScriptStore>();
            }
            return services.AddScoped<IScriptStore, ScriptStore>();
        }
    }
}