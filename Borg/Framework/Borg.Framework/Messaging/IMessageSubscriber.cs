using Newtonsoft.Json.Linq;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Borg.Infra.Messaging
{
    public interface IMessageSubscriber
    {
        void Subscribe<T>(Func<T, CancellationToken, Task> handler,
            CancellationToken cancellationToken = default) where T : class;
    }

    public interface IQueSubscriber<in T>
    {
        string QueName { get; }
        Func<T, CancellationToken, Task> Handler { get; }
    }

    public interface IJasonQueSubscriber : IQueSubscriber<JObject>
    {
    }
}