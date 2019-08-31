using Borg.Framework.Storage.Contracts;
using System;
using System.Threading.Tasks;

namespace Borg.Framework.Storage.Assets.Contracts
{
    public interface IAssetDirectoryStrategy<in TKey> where TKey : IEquatable<TKey>
    {
        Task<string> ParentFolder(IFileSpec<TKey> file);
    }
}