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
            if (stream == null || stream.Length == 0)
            {
                _data = new byte[0];
            }
            else
            {
                using (var memoryStream = new MemoryStream())
                {
                    stream.Position = 0;
                    stream.CopyTo(memoryStream);
                    _data = memoryStream.ToArray();
                }
            }
            Exists = true;
        }
        public DateTime LastWriteTimeUtc => LastModified.ToUniversalTime().DateTime;
        public string Extension => Path.GetExtension(PhysicalPath);
        public override long Length => _data.Length;
        public override bool IsDirectory => false;
    }
}