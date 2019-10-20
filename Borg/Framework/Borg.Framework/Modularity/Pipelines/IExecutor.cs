using System;
using System.Threading;
using System.Threading.Tasks;

namespace Borg.Framework.Modularity.Pipelines
{
    public interface IExecutor
    {
        event EventHandler<ExecutorEventArgs> OnExecuting;

        event EventHandler<ExecutorEventArgs> OnExecuted;

        Task Execute(CancellationToken cancelationToken = default);
    }
}