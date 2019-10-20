using Borg.Infrastructure.Core;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Threading;
using System.Threading.Tasks;

namespace Borg.Framework.Caching
{
    public class EvictionRequestHandler : IRequestHandler<EvictionRequest>
    {
        protected readonly ICacheClient cache;
        private readonly ILogger logger;

        public EvictionRequestHandler(ICacheClient cache, ILoggerFactory loggerFactory = null)
        {
            this.cache = Preconditions.NotNull(cache, nameof(cache));
            this.logger = loggerFactory == null ? NullLogger.Instance : loggerFactory.CreateLogger(GetType());
        }

        public async Task<Unit> Handle(EvictionRequest request, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (request != null)
            {
                await cache.Evict(request.Key, cancellationToken);
            }
            else
            {
                logger.Trace($"Eviction request is null");
            }
            return Unit.Value;
        }
    }
}