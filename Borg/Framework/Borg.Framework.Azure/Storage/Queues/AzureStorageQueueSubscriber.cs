using Borg.Infra.Messaging;
using Newtonsoft.Json.Linq;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Borg.Framework.Azure.Storage.Queues
{
    public abstract class AzureStorageQueueSubscriber : IJasonQueSubscriber
    {
        private readonly string _name;
        private readonly Func<JObject, CancellationToken, Task> _handler;

        protected AzureStorageQueueSubscriber(string name, Func<JObject, CancellationToken, Task> handler)
        {
            _name = name;
            _handler = handler;
        }

        public string QueName => _name;

        public Func<JObject, CancellationToken, Task> Handler => (o, c) =>
        {
            if (_handler != null) return _handler.Invoke(o, c);
            return Execute(o, c);
        };

        protected abstract Task Execute(JObject obj, CancellationToken cancellationToken);
    }
}