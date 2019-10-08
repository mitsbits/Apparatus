using Microsoft.Extensions.FileProviders;

namespace Borg.Framework.Storage.Contracts
{
    public interface IFileDepot : IFileStore, IFileProvider
    {

    }
}