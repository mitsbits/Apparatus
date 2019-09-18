using Borg.Infrastructure.Core.DDD.Contracts;

namespace Borg.Framework.Modularity.Pipelines
{
    public interface IPipelineStep<out TPipeline> : IHaveWeight, IExecutor where TPipeline : IPipeline
    {
    }
}