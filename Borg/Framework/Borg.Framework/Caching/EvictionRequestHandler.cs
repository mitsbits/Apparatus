using Borg.Framework.Dispatch.Contracts;
using Borg.Infrastructure.Core;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Threading;
using System.Threading.Tasks;

namespace Borg.Framework.Caching
{
    public class EvictionRequestHandler : AsyncNotificationHandler<EvictionRequest>
    {
        protected readonly ICacheClient cache;
        private readonly ILogger logger;

        public EvictionRequestHandler(ICacheClient cache, ILoggerFactory loggerFactory = null)
        {
            this.cache = Preconditions.NotNull(cache, nameof(cache));
            this.logger = loggerFactory == null ? NullLogger.Instance : loggerFactory.CreateLogger(GetType());
        }

        public override async Task Handle(object notification, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var message = notification as EvictionRequest;
            if (message != null)
            {
                await cache.Evict(message.Key, cancellationToken);
            }
            else
            {
                logger.Trace($"Eviction request is null");
            }
        }
    }
}