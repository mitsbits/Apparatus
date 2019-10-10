using Borg.Infrastructure.Core;
using Microsoft.Extensions.FileProviders;
using System.Collections;
using System.Collections.Generic;

namespace Borg.Framework.Storage.FileProviders
{
    public class MemoryDirectoryContents : IDirectoryContents
    {
        private readonly List<IFileInfo> _data;

        public MemoryDirectoryContents(IEnumerable<IFileInfo> data)
        {
            _data = new List<IFileInfo>(Preconditions.NotNull(data, nameof(data)));
            Exists = true;
        }

        private MemoryDirectoryContents()
        {
        }
        public bool Exists { get; private set; }

        public IEnumerator<IFileInfo> GetEnumerator() => _data.GetEnumerator();


        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}