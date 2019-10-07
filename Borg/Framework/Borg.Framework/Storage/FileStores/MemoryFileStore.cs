using Borg.Framework.Storage.Contracts;
using Borg.Framework.Storage.FileProviders;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Borg.Framework.Storage.FileStores
{
    public class MemoryFileStore : IFileStore
    {
        private readonly Lazy<ConcurrentDictionary<string, IFileInfo>> _source = new Lazy<ConcurrentDictionary<string, IFileInfo>>(() =>
        {
            var source = new ConcurrentDictionary<string, IFileInfo>();
            source.TryAdd("/", new MemoryDirectoryInfo());
            return source;
        });

        private ConcurrentDictionary<string, IFileInfo> Source => _source.Value;

        private static string SanitizePath(string source)
        {
            var output = source.ToLower().Trim();
            if (string.IsNullOrWhiteSpace(output)) { return "/"; };
            output = output.Replace(@"\", "/").Replace(@"\\", "/").Replace("//", "/").TrimEnd('/');
            if (Path.GetExtension(output).Length == 0)
            {
                output = $"{output}/";
            }
            return output;
        }
        public MemoryFileStore() : this(1024 * 1024 * 256, 100)
        {
        }

        public MemoryFileStore(long maxFileSize, int maxFiles)
        {
            MaxFileSize = maxFileSize;
            MaxFiles = maxFiles;
        }

        public long MaxFileSize { get; private set; }
        public long MaxFiles { get; private set; }




        public Task<bool> Delete(string path, CancellationToken cancellationToken = default)
        {
            path = SanitizePath(path);
            if (Source.ContainsKey(path))
            {
                var result = Source.TryRemove(path, out var value);
                return Task.FromResult(result);
            }
            return Task.FromResult(false);
        }

        public Task<bool> Exists(string path, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Source.ContainsKey(SanitizePath(path)));
        }

        public Task<bool> Save(string path, Stream stream, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            path = SanitizePath(path);
            if (Source.TryGetValue(path, out var retrieved))
            {
                return Task.FromResult(Source.TryUpdate(path, CreateFromInput(path, stream), retrieved));
            }
            return Task.FromResult(Source.TryAdd(path, CreateFromInput(path, stream)));
        }


        private static IFileInfo CreateFromInput(string path, Stream stream)
        {

        }
    }
}