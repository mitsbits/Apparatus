using System.Threading.Tasks;

namespace Borg.Infrastructure.Core.Reflection.Discovery
{
    public interface IAssemblyScanner
    {
        Task<AssemblyScanResult> Scan();
    }
}