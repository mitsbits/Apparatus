using Autofac;
using Borg.Framework.Dispatch.Contracts;
using Borg.Infrastructure.Core.DDD.ValueObjects;
using Borg.Infrastructure.Core.Threading;
using Shouldly;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Borg.Platform.Dispatch.Tests.NetCore
{
    public class SimpleCommandTest : DispatchTestBase
    {
        private static int Target { get; set; } = 1;

        public SimpleCommandTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task a_simple_request()
        {
            var before = Target;

            using (var scope = Provider.CreateScope())
            {
                var dispatcher = scope.ServiceProvider.GetRequiredService<IDispatcher>();
                await dispatcher.Send(new RaiseTheTargetRequest());
            }
            var after = Target;
            after.ShouldBe(before + 1);
        }

        [Fact]
        public async Task a_simple_command()
        {
            var message = Guid.NewGuid().ToString();
            var greeting = string.Empty;

            using (var scope = Provider.CreateScope())
            {
                var dispatcher = scope.ServiceProvider.GetRequiredService<IDispatcher>();
                var response = await dispatcher.Send<GreetingCommand, GreetingCommandResponse>(new GreetingCommand(message));
                greeting = response.Greeting;
            }

            message.ShouldBe(greeting);
        }

        [Fact]
        public void fire_and_forget()
        {
            var before = Target;
            using (var scope = Provider.CreateScope())
            {
                var dispatcher = scope.ServiceProvider.GetRequiredService<IDispatcher>();
                dispatcher.Send(new RaiseTheTargetRequest());
            }
            Thread.Sleep(500);
            var after = Target;
            after.ShouldBe(before + 1);
        }

        [Fact]
        public async Task fire_and_forget_parallel()
        {
            var before = Target;
            using (var scope = Provider.CreateScope())
            {
                var dispatcher = scope.ServiceProvider.GetRequiredService<IDispatcher>();
                var tasks = new Task[]
                {
                    dispatcher.Send(new RaiseTheTargetRequest()),
                    dispatcher.Send(new RaiseTheTargetRequest()),
                    dispatcher.Send(new RaiseTheTargetRequest()),
                    dispatcher.Send(new RaiseTheTargetRequest()),
                    dispatcher.Send(new RaiseTheTargetRequest())
                };
                await Task.WhenAll(tasks);
            }

            var after = Target;
            after.ShouldBe(before + 5);
        }

        protected override void RegisterSpecificServices(IServiceCollection services)
        {
            base.RegisterSpecificServices(services);
            services.AddScoped<IRequestHandler<RaiseTheTargetRequest>, RaiseTheTargetRequestHandler>();
            services.AddScoped<IRequestHandler<GreetingCommand, GreetingCommandResponse>, GreetingCommandHandler>();

        }

        #region test_a_simple_request

        public class RaiseTheTargetRequest
        {
        }

        public class RaiseTheTargetRequestHandler : RequestHandler<RaiseTheTargetRequest>
        {
            private AsyncLock _lock;

            public RaiseTheTargetRequestHandler()
            {
                _lock = new AsyncLock();
            }

            public override object Handle(object request)
            {
                using (_lock.Lock())
                {
                    var simple = request as RaiseTheTargetRequest;
                    if (simple == null) throw new InvalidOperationException($"Requested {nameof(RaiseTheTargetRequest)} but {request.GetType().Name} was provided");
                    Target++;
                }
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