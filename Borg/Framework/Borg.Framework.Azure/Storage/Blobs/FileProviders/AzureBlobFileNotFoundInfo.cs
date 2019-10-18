using Borg.Infrastructure.Core;
using Microsoft.Extensions.FileProviders;
using System;
using System.IO;

namespace Borg.Framework.Azure.Storage.Blobs.FileProviders
{
    public class AzureBlobFileNotFoundInfo : IFileInfo
    {
        

        public AzureBlobFileNotFoundInfo(string path)
        {
            PhysicalPath = Preconditions.NotEmpty(path, nameof(path));
            Name = Path.GetFileNameWithoutExtension(PhysicalPath);
        }

        public virtual long Length => -1;

        public virtual string PhysicalPath { get;}

        public virtual string Name { get; }

        public  bool Exists => false;

        public  Stream CreateReadStream()
        {
            throw new InvalidOperationException("Can not read a file that doesnt exist");
        }
        public  DateTimeOffset LastModified => DateTimeOffset.MinValue;

        public bool IsDirectory => false;
    }
}