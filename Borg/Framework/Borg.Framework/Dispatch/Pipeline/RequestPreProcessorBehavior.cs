namespace Borg.Framework.Dispatch.Pipeline
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public class RequestPreProcessorBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IEnumerable<IRequestPreProcessor<TRequest>> _preProcessors;

        public RequestPreProcessorBehavior(IEnumerable<IRequestPreProcessor<TRequest>> preProcessors)
        {
            _preProcessors = preProcessors;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            foreach (var processor in _preProcessors)
            {
                await processor.Process(request, cancellationToken).AnyContext();
            }

            return await next().AnyContext();
        }
    }
}