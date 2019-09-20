using Autofac;
using Borg.Framework.Dispatch.Contracts;
using Borg.Infrastructure.Core.DDD.ValueObjects;
using Microsoft.Extensions.Logging;
using Shouldly;
using System;
using System.Threading.Tasks;
using Test.Borg;
using Xunit;
using Xunit.Abstractions;
using IContainer = Autofac.IContainer;

namespace Borg.Platform.Dispatch.Autofac.Tests
{
    public class SimpleCommandTest : TestBase
    {
        private static IContainer Container { get; set; }

        private static int Target { get; set; } = 1;

        public SimpleCommandTest(ITestOutputHelper output) : base(output)
        {
            Container = BuildContainer();
        }

        [Fact]
        public async Task test_a_simplle_command()
        {
            var before = Target;

            using (var scope = Container.BeginLifetimeScope())
            {
                var dispatcher = scope.Resolve<IDispatcher>();
                await dispatcher.Send(new RaiseTheTargetRequest());
            }
            var after = Target;
            after.ShouldBe(before + 1);
        }

        private IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<Dispatcher>().As<IDispatcher>().SingleInstance();
            builder.RegisterType<ServiceFactory>().SingleInstance();
            builder.RegisterType<RaiseTheTargetRequestHandler>().As<IRequestHandler<RaiseTheTargetRequest>>();
            builder.RegisterInstance(_moqLoggerFactory).As<ILoggerFactory>();
            return builder.Build();
        }

        public class RaiseTheTargetRequest
        {
        }

        public class RaiseTheTargetRequestHandler : RequestHandler<RaiseTheTargetRequest>
        {
            public RaiseTheTargetRequestHandler()
            {
            }

            public override object Handle(object request)
            {
                var simple = request as RaiseTheTargetRequest;
                if (simple == null) throw new InvalidOperationException($"Requested {nameof(RaiseTheTargetRequest)} but {request.GetType().Name} was provided");
                Target++;
                return Unit.Value;
            }
        }
    }
}