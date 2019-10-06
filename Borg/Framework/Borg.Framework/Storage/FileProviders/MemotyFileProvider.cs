using Borg.Infrastructure.Core;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;

namespace Borg.Framework.Storage.FileProviders
{
    public class MemotyFileProvider : IFileProvider
    {
        private readonly Lazy<ConcurrentDictionary<string, IFileInfo>> _source = new Lazy<ConcurrentDictionary<string, IFileInfo>>(() =>
        {
            var source = new ConcurrentDictionary<string, IFileInfo>();
            source.TryAdd("/", new MemoryDirectoryInfo());
        });

        private ConcurrentDictionary<string, IFileInfo> Source => _source.Value;

        public IDirectoryContents GetDirectoryContents(string subpath)
        {
            subpath = subpath.Trim();
            if (string.IsNullOrWhiteSpace(subpath)) subpath = "/";
            var bucket = new List<IFileInfo>();
            foreach (var key in Source.Keys)
            {
                if (key.StartsWith(subpath))
                {
                    bucket.Add(Source[key]);
                }
            }
            return new MemoryDirectoryContents(bucket);
        }

        public IFileInfo GetFileInfo(string subpath)
        {
            throw new NotImplementedException();
        }

        public IChangeToken Watch(string filter)
        {
            throw new NotImplementedException();
        }
    }

    public abstract class MemoryInfo : IFileInfo
    {
        private readonly byte[] _data;
        public bool Exists { get; protected set; }

        public long Length { get; protected set; }

        public string PhysicalPath { get; protected set; }

        public string Name { get; protected set; }

        public DateTimeOffset LastModified { get; protected set; }

        public virtual bool IsDirectory { get; protected set; }

        public virtual Stream CreateReadStream()
        {
            var stream = new MemoryStream(_data);
            stream.Position = 0;
            return stream;
        }
    }

    public class MemoryDirectoryInfo : MemoryInfo
    {
        public override bool IsDirectory => true;

        public override Stream CreateReadStream()
        {
            return new MemoryStream();
        }
    }

    public class MemoryFileInfo : MemoryInfo
    {
        public override bool IsDirectory => false;
    }

    public class MemoryDirectoryContents : IDirectoryContents
    {
        private readonly List<IFileInfo> _data;

        public MemoryDirectoryContents(IEnumerable<IFileInfo> data)
        {
            _data = new List<IFileInfo>(Preconditions.NotNull(data, nameof(data)));
        }

        private MemoryDirectoryContents()
        {
        }

        public static IDirectoryContents NotFound()
        {
            return new MemoryDirectoryContents() { Exists = false };
        }

        public bool Exists { get; private set; }

        public IEnumerator<IFileInfo> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}