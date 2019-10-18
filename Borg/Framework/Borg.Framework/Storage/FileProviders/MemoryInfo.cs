using Microsoft.Extensions.FileProviders;
using System;
using System.IO;

namespace Borg.Framework.Storage.FileProviders
{
    public abstract class MemoryInfo : IFileInfo
    {
        protected byte[] _data;
        public virtual bool Exists { get; protected set; }

        public virtual long Length { get; protected set; }

        public virtual string PhysicalPath { get; protected set; }

        public virtual string Name { get; protected set; }

        public virtual DateTimeOffset LastModified { get; protected set; }

        public virtual bool IsDirectory { get; protected set; }

        public virtual Stream CreateReadStream()
        {
            var stream = new MemoryStream(_data);
            stream.Position = 0;
            return stream;
        }
    }
}