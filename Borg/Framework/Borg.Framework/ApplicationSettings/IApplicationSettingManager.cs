using System.Threading.Tasks;

namespace Borg.Framework.ApplicationSettings
{
    public interface IApplicationSettingManager<T> where T : IApplicationSetting, new()
    {
        Task<T> Get();

        Task<T> UpdateOrCreate(T updated);
    }
}