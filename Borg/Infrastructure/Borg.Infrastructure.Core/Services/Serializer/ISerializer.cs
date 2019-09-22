using System.Threading.Tasks;

namespace Borg.Infrastructure.Core.Services.Serializer
{
    public interface ISerializer
    {
        Task<object> Deserialize(byte[] data);

        Task<byte[]> Serialize(object value);
    }
}