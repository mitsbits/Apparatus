using Microsoft.WindowsAzure.Storage.Blob;

namespace Borg.Framework.Azure.Storage.Blobs
{
    public interface IBlobContainerFactory
    {
        CloudBlobContainer GetContainer(string subpath);
        string TransformPath(string subpath);
    }
}
