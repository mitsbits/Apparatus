using System;
using System.Threading;
using System.Threading.Tasks;

namespace Borg.Framework.Modularity.Pipelines
{
    public interface IExecutor
    {
        event EventHandler OnExecuting;
        event EventHandler OnExecuted;
        Task Execute(CancellationToken cancelationToken);
    }
}