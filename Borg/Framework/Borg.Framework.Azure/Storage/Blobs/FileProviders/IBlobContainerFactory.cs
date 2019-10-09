using Microsoft.WindowsAzure.Storage.Blob;

namespace Borg.Framework.Azure.Storage.Blobs
{
    public interface IBlobContainerFactory
    {
        CloudBlobContainer GetContainer();
        string TransformPath(string subpath);
    }
}
