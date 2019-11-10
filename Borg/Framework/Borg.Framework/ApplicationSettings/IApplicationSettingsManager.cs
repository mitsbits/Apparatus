using System.Threading.Tasks;

namespace Borg.Framework.ApplicationSettings
{
    public interface IApplicationSettingsManager
    {
        Task<T> Get<T>() where T : IApplicationSetting, new();

        Task<T> UpdateOrCreate<T>(T updated) where T : IApplicationSetting, new();
    }
}