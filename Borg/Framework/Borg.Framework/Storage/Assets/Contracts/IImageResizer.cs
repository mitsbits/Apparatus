using System.IO;
using System.Threading.Tasks;

namespace Borg.Framework.Storage.Assets.Contracts
{
    public interface IImageResizer
    {
        Task<Stream> ResizeByLargeSide(Stream input, int sizeInPixels, string mime);
    }
}