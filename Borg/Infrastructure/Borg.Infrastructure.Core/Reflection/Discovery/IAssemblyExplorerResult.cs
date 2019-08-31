using System.Collections.Generic;

namespace Borg.Infrastructure.Core.Reflection.Discovery
{
    public interface IAssemblyExplorerResult
    {
        IEnumerable<AssemblyScanResult> Results { get; }
    }
}