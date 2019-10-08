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
        }

        private MemoryDirectoryContents()
        {
        }

        public static IDirectoryContents NotFound()
        {
            return new MemoryDirectoryContents() { Exists = false };
        }

        public bool Exists { get; private set; }

        public IEnumerator<IFileInfo> GetEnumerator() => _data.GetEnumerator();


        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}