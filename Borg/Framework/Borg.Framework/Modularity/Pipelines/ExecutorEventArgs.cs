using System;

namespace Borg.Framework.Modularity.Pipelines
{
    public class ExecutorEventArgs : EventArgs
    {
        public ExecutorEventArgs() : base()
        {
        }

        public static Lazy<ExecutorEventArgs> empty = new Lazy<ExecutorEventArgs>(() =>
        {
            return new ExecutorEventArgs();
        });

        public new static ExecutorEventArgs Empty => empty.Value;
    }
}