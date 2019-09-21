using Autofac;
using Borg.Framework.Dispatch.Contracts;
using Borg.Infrastructure.Core.DDD.ValueObjects;
using Shouldly;
using System;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Borg.Platform.Dispatch.Autofac.Tests
{
    public class SimpleCommandTest : DispatchTestBase
    {
        private static int Target { get; set; } = 1;

        public SimpleCommandTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task test_a_simple_request()
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

        [Fact]
        public async Task test_a_simple_command()
        {
            var message = Guid.NewGuid().ToString();
            var greeting = string.Empty;

            using (var scope = Container.BeginLifetimeScope())
            {
                var dispatcher = scope.Resolve<IDispatcher>();
                var response = await dispatcher.Send<GreetingCommand, GreetingCommandResponse>(new GreetingCommand(message));
                greeting = response.Greeting;
            }

            message.ShouldBe(greeting);
        }

        protected override void RegisterSpecificServices(ContainerBuilder builder)
        {
            base.RegisterSpecificServices(builder);
            builder.RegisterType<RaiseTheTargetRequestHandler>().As<IRequestHandler<RaiseTheTargetRequest>>();
            builder.RegisterType<GreetingCommandHandler>().As<IRequestHandler<GreetingCommand, GreetingCommandResponse>>();
        }

        #region test_a_simple_request

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

        #endregion test_a_simple_request

        #region test_a_simple_command

        public class GreetingCommand
        {
            public GreetingCommand(string greeting)
            {
                Greeting = greeting;
            }

            public string Greeting { get; }
        }

        public class GreetingCommandResponse
        {
            public GreetingCommandResponse(string greeting)
            {
                Greeting = greeting;
            }

            public string Greeting { get; }
        }

        public class GreetingCommandHandler : RequestHandler<GreetingCommand, GreetingCommandResponse>
        {
            public override object Handle(object request)
            {
                var command = request as GreetingCommand;
                if (command == null) throw new InvalidOperationException($"Requested {nameof(RaiseTheTargetRequest)} but {request.GetType().Name} was provided");
                return new GreetingCommandResponse(command.Greeting);
            }
        }

        #endregion test_a_simple_command
    }
}