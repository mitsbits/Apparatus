using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Borg.Framework.Modularity.Pipelines
{
    public abstract class GenericHostStartUp : IHostStartUpJob
    {
        protected readonly ICollection<IPipelineStep<IPipeline>> source = new HashSet<IPipelineStep<IPipeline>>();

        public event EventHandler OnExecuting;
        public event EventHandler OnExecuted;

        public async Task Execute(CancellationToken cancelationToken)
        {
            cancelationToken.ThrowIfCancellationRequested();
            OnExecuting?.Invoke(this, EventArgs.Empty);
            foreach (var step in this.LightToHeavy())
            {
                await step.Execute(cancelationToken);
            }
            OnExecuted?.Invoke(this, EventArgs.Empty);
        }

        public IEnumerator<IPipelineStep<IPipeline>> GetEnumerator()
        {
            return source.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public abstract class GenericPipelineStep<TPipeline> : IPipelineStep<TPipeline> where TPipeline : IPipeline
    {
        public event EventHandler OnExecuting;
        public event EventHandler OnExecuted;

        public virtual double Weight { get; set; } = 0;
        public async Task Execute(CancellationToken cancelationToken)
        {

            OnExecuting?.Invoke(this, EventArgs.Empty);
            await ExecuteInternal(cancelationToken);
            OnExecuted?.Invoke(this, EventArgs.Empty);
        }
        protected abstract Task ExecuteInternal(CancellationToken cancellationToken);

    }
}