using System;
using System.Collections.Generic;
using System.Text;

namespace Borg.Framework.Modularity.Pipelines
{
    public interface IPipeline : IEnumerable<IPipelineStep<IPipeline>>
    {
    }
}
