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
            Name = path.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries).Last();
        }

        public override long Length => 0;
        public override bool IsDirectory => true;

        public override Stream CreateReadStream()
        {
            return new MemoryStream();
        }
    }
}