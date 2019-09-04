using Autofac;
using Borg.Framework.Dispatch.Contracts;
using Borg.Platform.Dispatch.Autofac;

namespace Borg
{
    public class BorgPlatformDispatchAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ServiceFactory>().SingleInstance();
            builder.RegisterType<Dispatcher>().As<IDispatcher>().SingleInstance();
        }
    }
}