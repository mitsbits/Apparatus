using Borg.Framework;
using Borg.Framework.Dispatch.Contracts;

using Borg.Platform.Dispatch.NetCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using Test.Borg;
using Xunit.Abstractions;

namespace Borg.Platform.Dispatch.Tests.NetCore
{
    public class DispatchTestBase : TestBase
    {
        protected DispatchTestBase(ITestOutputHelper output) : base(output)
        {
            Provider = BuildProvider(_moqLoggerFactory);
        }

        protected IServiceProvider Provider { get; set; }

        protected ServiceProvider BuildProvider(ILoggerFactory loggerFactory)
        {
            var serviceProviderBuilder = new ServiceCollection()
                 .AddSingleton<IDispatcher, Dispatcher>()
                 .AddSingleton<ServiceFactory>()
                 .AddSingleton<ILoggerFactory>(loggerFactory);
            RegisterSpecificServices(serviceProviderBuilder);
            var provider = serviceProviderBuilder.BuildServiceProvider();
            ServiceLocator.SetLocatorProvider(provider);
            return provider;
        }

        protected virtual void RegisterSpecificServices(IServiceCollection services)
        {
        }
    }
}