using System.Threading.Tasks;

namespace Borg.Framework.Storage.Assets.Contracts
{
    public interface IConflictingNamesResolver
    {
        Task<string> Resolve(string filename);
    }
}