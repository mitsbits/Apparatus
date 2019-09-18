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

        public event EventHandler<ExecutorEventArgs> OnExecuting;

        public event EventHandler<ExecutorEventArgs> OnExecuted;

        public async Task Execute(CancellationToken cancelationToken)
        {
            cancelationToken.ThrowIfCancellationRequested();
            OnExecuting?.Invoke(this, ExecutorEventArgs.Empty);
            foreach (var step in this.LightToHeavy())
            {
                await step.Execute(cancelationToken);
            }
            OnExecuted?.Invoke(this, ExecutorEventArgs.Empty);
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
        public event EventHandler<ExecutorEventArgs> OnExecuting;

        public event EventHandler<ExecutorEventArgs> OnExecuted;

        public virtual double Weight { get; set; } = 0;

        public async Task Execute(CancellationToken cancelationToken)
        {
            OnExecuting?.Invoke(this, ExecutorEventArgs.Empty);
            await ExecuteInternal(cancelationToken);
            OnExecuted?.Invoke(this, ExecutorEventArgs.Empty);
        }

        protected abstract Task ExecuteInternal(CancellationToken cancelationToken);
    }
}