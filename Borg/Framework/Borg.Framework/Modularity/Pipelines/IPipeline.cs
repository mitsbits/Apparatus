using System.Collections.Generic;

namespace Borg.Framework.Modularity.Pipelines
{
    public interface IPipeline : IEnumerable<IPipelineStep<IPipeline>>
    {
    }
}