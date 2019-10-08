using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Borg.Framework.Storage.Contracts
{
    public interface IFileStore
    {
        Task<bool> Exists(string path, CancellationToken cancellationToken = default);

        Task<bool> Save(string path, Stream stream, CancellationToken cancellationToken = default);

        

        Task<bool> Delete(string path, CancellationToken cancellationToken = default);
    }
}