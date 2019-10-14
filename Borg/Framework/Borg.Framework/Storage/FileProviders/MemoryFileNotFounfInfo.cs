using Borg.Infrastructure.Core;
using System;
using System.IO;

namespace Borg.Framework.Storage.FileProviders
{
    public class MemoryFileNotFounfInfo : MemoryInfo
    {
        public MemoryFileNotFounfInfo(string path)
        {
            PhysicalPath = Preconditions.NotEmpty(path, nameof(path));       
        }
        public override bool Exists => false;
        public override long Length => 0;
        public override Stream CreateReadStream()
        {
            throw new InvalidOperationException("Can not read a file that doesnt exist");
        }
    }
}