using Borg.Infrastructure.Core;
using System;
using System.IO;

namespace Borg.Framework.Storage.FileProviders
{
    public class MemoryFileInfo : MemoryInfo
    {
        public MemoryFileInfo(string path, Stream stream)
        {
            path = Preconditions.NotEmpty(path, nameof(path));
            PhysicalPath = path;
            Name = Path.GetFileNameWithoutExtension(path);
            LastModified = DateTimeOffset.Now;
            using (var memoryStream = new MemoryStream())
            {
                stream.Position = 0;
                stream.CopyTo(memoryStream);
                _data = memoryStream.ToArray();
            }
        }

        public override long Length => _data.Length;
        public override bool IsDirectory => false;
    }
}