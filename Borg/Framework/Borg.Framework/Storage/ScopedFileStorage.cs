using Borg.Framework.Storage.Contracts;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Borg.Framework.Storage
{
    public class ScopedFileStorage : IFileStorage, IScopedFileStorage
    {
        private readonly string _pathPrefix;

        public ScopedFileStorage(IFileStorage storage, string scope)
        {
            UnscopedStorage = storage;
            Scope = !string.IsNullOrWhiteSpace(scope) ? scope.Trim().TrimEnd('/') : null;
            _pathPrefix = Scope != null ? string.Concat(Scope, "/") : string.Empty;
        }

        public IFileStorage UnscopedStorage { get; }

        public Task<Stream> GetFileStream(string path, CancellationToken cancellationToken = new CancellationToken())
        {
            return UnscopedStorage.GetFileStream(string.Concat(_pathPrefix, path), cancellationToken);
        }

        public async Task<IFileSpec> GetFileInfo(string path,
            CancellationToken cancellationToken = new CancellationToken())
        {
            var file = await UnscopedStorage.GetFileInfo(string.Concat(_pathPrefix, path), cancellationToken);
            var name = file.Name;
            if (name.StartsWith(_pathPrefix)) name = name.Substring(_pathPrefix.Length);
            return new FileSpecDefinition(file.FullPath, name, file.CreationDate, file.LastWrite, file.LastRead, file.SizeInBytes,
                file.MimeType);
        }

        public Task<bool> Exists(string path, CancellationToken cancellationToken = new CancellationToken())
        {
            return UnscopedStorage.Exists(string.Concat(_pathPrefix, path));
        }

        public Task<bool> Save(string path, Stream stream,
            CancellationToken cancellationToken = new CancellationToken())
        {
            return UnscopedStorage.Save(string.Concat(_pathPrefix, path), stream, cancellationToken);
        }

        public Task<bool> CopyFile(string path, string targetpath,
            CancellationToken cancellationToken = new CancellationToken())
        {
            //return UnscopedStorage.CopyFile(string.Concat(_pathPrefix, path), string.Concat(_pathPrefix, targetpath),
            //    cancellationToken);
            return Task.FromResult(false);
        }

        public Task<bool> Delete(string path, CancellationToken cancellationToken = new CancellationToken())
        {
            return UnscopedStorage.Delete(string.Concat(_pathPrefix, path), cancellationToken);
        }

        public async Task<IEnumerable<IFileSpec>> GetFileList(string searchPattern = null, int? limit = null,
            int? skip = null, CancellationToken cancellationToken = new CancellationToken())
        {
            if (string.IsNullOrEmpty(searchPattern))
                searchPattern = "*";
            var files = (await UnscopedStorage
                    .GetFileList(string.Concat(_pathPrefix, searchPattern), limit, skip, cancellationToken)
                    .AnyContext())
                .ToList();
            return files.Select(x => new FileSpecDefinition(x.FullPath,
                x.Name.StartsWith(_pathPrefix) ? x.Name.Substring(_pathPrefix.Length) : x.Name, x.CreationDate,
                x.LastWrite, x.LastRead, x.SizeInBytes, x.MimeType));
        }

        public void Dispose()
        {
        }

        public IChangeToken Watch(string filter)
        {
            throw new System.NotImplementedException();
        }

        public string Scope { get; }
    }
}