using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using System;

namespace Borg.Framework.Storage
{
    public class StorageFileProvider : IFileProvider
    {
        public IDirectoryContents GetDirectoryContents(string subpath)
        {
            throw new NotImplementedException();
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
}