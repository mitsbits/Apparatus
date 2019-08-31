using System.Collections.Generic;
using System.Threading.Tasks;

namespace Borg.Infrastructure.Core.Reflection.Discovery
{
    public interface IAssemblyExplorer
    {
        IEnumerable<AssemblyScanResult> Results();

        bool ScanCompleted { get; }

        Task Scan();
    }
}