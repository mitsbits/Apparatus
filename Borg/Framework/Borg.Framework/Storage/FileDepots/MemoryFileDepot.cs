using Borg.Framework.Storage.Contracts;
using Borg.Framework.Storage.FileProviders;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Borg.Framework.Storage.FileDepots
{
    public class MemoryFileDepot : IFileDepot, IDisposable
    {
        private readonly Lazy<ConcurrentDictionary<string, IFileInfo>> _source = new Lazy<ConcurrentDictionary<string, IFileInfo>>(() =>
        {
            var source = new ConcurrentDictionary<string, IFileInfo>();
            source.TryAdd("/", new MemoryDirectoryInfo("/"));
            return source;
        });

        private ConcurrentDictionary<string, IFileInfo> Source => _source.Value;

        public MemoryFileDepot() : this(1024 * 1024 * 256, 100)
        {
        }

        public MemoryFileDepot(long maxFileSize, int maxFiles)
        {
            MaxFileSize = maxFileSize;
            MaxFiles = maxFiles;
        }

        private long MaxFileSize { get; }
        private long MaxFiles { get; }

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

        public IDirectoryContents GetDirectoryContents(string subpath)
        {
            subpath = SanitizePath(subpath);
            var bucket = new List<IFileInfo>();
            foreach (var key in Source.Keys)
            {
                if (key.StartsWith(subpath, true, CultureInfo.InvariantCulture))
                {
                    bucket.Add(Source[key]);
                }
            }
            return new MemoryDirectoryContents(bucket);
        }

        public IFileInfo GetFileInfo(string subpath)
        {
            subpath = SanitizePath(subpath);
            if (Source.TryGetValue(subpath, out var fileInfo))
            {
                return fileInfo;
            }
            return null;
        }

        public IChangeToken Watch(string filter)
        {
            throw new NotImplementedException();
        }

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

        private IFileInfo CreateFromInput(string path, Stream stream, [CallerMemberName] string callerName = "")
        {
            byte[] data;
            using (var memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                data = memoryStream.ToArray();
            }
            path = SanitizePath(path);
            if (callerName == nameof(Save))
            {
                if (data.Length > MaxFileSize) { throw new InvalidOperationException($"{nameof(MemoryFileDepot)} - Max file size is {MaxFileSize.SizeDisplay()} but the file at {path} is {data.Length.SizeDisplay()}"); }
                if (Source.Count == MaxFiles) { throw new InvalidOperationException($"{nameof(MemoryFileDepot)} - Max file count of {MaxFiles} reached, can not add file at {path}. Remove some and try again."); }
            }
            var directory = EnsureDirectory(path, out var filename);
            if (filename.IsNullOrWhiteSpace())
            {
                return directory;
            }

            return new MemoryFileInfo(path, stream);
        }

        private IFileInfo EnsureDirectory(string path, out string filename)
        {
            path = SanitizePath(path);
            var parts = path.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

            var lastEntryIsFile = !Path.GetExtension(parts.Last()).Trim().IsNullOrWhiteSpace();
            filename = lastEntryIsFile ? parts.Last() : string.Empty;
            var tree = string.Empty;
            foreach (var part in parts.Take(parts.Length - (lastEntryIsFile ? 1 : 0)))
            {
                tree = SanitizePath($"{tree}/{part}/");
                var directory = new MemoryDirectoryInfo(tree);
                Source.TryAdd(tree, directory);
            }
            return Source[tree];
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
           
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~MemoryFileDepot()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        void IDisposable.Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}