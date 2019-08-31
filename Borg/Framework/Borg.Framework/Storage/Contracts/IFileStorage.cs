using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Borg.Framework.Storage.Contracts
{
    public interface IFileStorage : IDisposable
    {
        Task<Stream> GetFileStream(string path, CancellationToken cancellationToken = default);

        Task<IFileSpec> GetFileInfo(string path, CancellationToken cancellationToken = default);

        Task<bool> Exists(string path, CancellationToken cancellationToken = default);

        Task<bool> SaveFile(string path, Stream stream, CancellationToken cancellationToken = default);

        Task<bool> CopyFile(string path, string targetpath, CancellationToken cancellationToken = default);

        Task<bool> DeleteFile(string path, CancellationToken cancellationToken = default);

        Task<IEnumerable<IFileSpec>> GetFileList(string searchPattern = null, int? limit = null, int? skip = null,
            CancellationToken cancellationToken = default);
        IChangeToken Watch(string filter);
    }

    public interface IScopedFileStorage : IFileStorage
    {
        string Scope { get; }
    }
}