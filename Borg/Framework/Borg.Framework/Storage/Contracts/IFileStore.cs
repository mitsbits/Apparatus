using Microsoft.Extensions.FileProviders;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Borg.Framework.Storage.Contracts
{
    public interface IFileStore
    {
        Task<bool> Exists(string path, CancellationToken cancellationToken = default);

        Task<IFileInfo> Save(string path, Stream stream, CancellationToken cancellationToken = default);

        Task Delete(string path, CancellationToken cancellationToken = default);
    }
}