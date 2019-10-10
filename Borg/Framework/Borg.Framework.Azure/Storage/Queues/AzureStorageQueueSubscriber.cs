using Borg.Infra.Messaging;

using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Borg.Framework.Azure.Storage.Queues
{
    public abstract class AzureStorageQueueSubscriber : IJasonQueSubscriber
    {
        private readonly string _name;
        private readonly Func<JsonDocument, CancellationToken, Task> _handler;

        protected AzureStorageQueueSubscriber(string name, Func<JsonDocument, CancellationToken, Task> handler)
        {
            _name = name;
            _handler = handler;
        }

        public string QueName => _name;

        public Func<JsonDocument, CancellationToken, Task> Handler => (o, c) =>
        {
            if (_handler != null) return _handler.Invoke(o, c);
            return Execute(o, c);
        };

        protected abstract Task Execute(JsonDocument obj, CancellationToken cancellationToken);
    }
}