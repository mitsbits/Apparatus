using Autofac;
using Borg.Framework.Dispatch.Contracts;
using Microsoft.Extensions.Logging;
using Test.Borg;
using Xunit.Abstractions;
using IContainer = Autofac.IContainer;

namespace Borg.Platform.Dispatch.Autofac.Tests
{
    public class DispatchTestBase : TestBase
    {
        protected DispatchTestBase(ITestOutputHelper output) : base(output)
        {
            Container = BuildContainer(_moqLoggerFactory);
        }
        protected  IContainer Container { get; set; }

        protected  IContainer BuildContainer(ILoggerFactory loggerFactory)
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<Dispatcher>().As<IDispatcher>().SingleInstance();
            builder.RegisterType<ServiceFactory>().SingleInstance();
          
            builder.RegisterInstance(loggerFactory).As<ILoggerFactory>();
            RegisterSpecificServices(builder);
            return builder.Build();
        }

        protected virtual void RegisterSpecificServices(ContainerBuilder builder) { }
    }
}