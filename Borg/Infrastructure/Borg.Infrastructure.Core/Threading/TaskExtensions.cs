using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Borg
{
    public static class TaskExtensions
    {
        [DebuggerStepThrough]
        public static ConfiguredTaskAwaitable<TResult> AnyContext<TResult>(this Task<TResult> task)
        {
            return task.ConfigureAwait(false);
        }

        [DebuggerStepThrough]
        public static ConfiguredTaskAwaitable AnyContext(this Task task)
        {
            return task.ConfigureAwait(false);
        }
        [DebuggerStepThrough]
        public static ConfiguredValueTaskAwaitable AnyContext(this ValueTask task)
        {
            return task.ConfigureAwait(false);
        }
        [DebuggerStepThrough]
        public static ConfiguredValueTaskAwaitable<TResult> AnyContext<TResult>(this ValueTask<TResult> task)
        {
            return task.ConfigureAwait(false);
        }

    }
}