using System.Reflection;

namespace Borg.Infrastructure.Core.Reflection.Discovery
{
    public abstract class AssemblyScanResult
    {
        protected AssemblyScanResult(Assembly assembly)
        {
            Assembly = Preconditions.NotNull(assembly, nameof(assembly));
        }

        protected AssemblyScanResult(Assembly assembly, bool success) : this(assembly)
        {
            Success = success;
        }

        protected AssemblyScanResult(Assembly assembly, bool success, string[] errors) : this(assembly, success)
        {
            Errors = errors;
        }

        public virtual Assembly Assembly { get; }
        public virtual string[] Errors { get; }
        public virtual bool Success { get; }
    }
}