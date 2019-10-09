using Borg.Infrastructure.Core;
using System;
using System.IO;
using System.Linq;

namespace Borg.Framework.Storage.FileProviders
{
    public class MemoryDirectoryInfo : MemoryInfo
    {
        public MemoryDirectoryInfo(string path)
        {
            path = Preconditions.NotEmpty(path, nameof(path));
            LastModified = DateTimeOffset.Now;
            PhysicalPath = path;
            var parts = path.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            Name = parts.Length == 0 ? string.Empty : parts.Last();
        }

        public override long Length => 0;
        public override bool IsDirectory => true;

        public override Stream CreateReadStream()
        {
            return new MemoryStream();
        }
    }
}