using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;

namespace Borg.Framework.Storage.FileProviders
{
    public class MemoryFileProvider : IFileProvider
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
            if (string.IsNullOrWhiteSpace(output)) output = "/";
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
    }
}