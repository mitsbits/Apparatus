using Borg.Infrastructure.Core;
using System;
using System.Threading.Tasks;

namespace Borg.Framework.ApplicationSettings
{
    public delegate void ApplicationSettingChangedEventHandler(object sender, ApplicationSettingChangedEventArgs e);
    public interface IApplicationSettingsManager
    {
        event ApplicationSettingChangedEventHandler ApplicationSettingChange;
        Task<T> Get<T>() where T : IApplicationSetting, new();

        Task<T> UpdateOrCreate<T>(T updated) where T : IApplicationSetting, new();
    }
    public class ApplicationSettingChangedEventArgs : EventArgs
    {
        public ApplicationSettingChangedEventArgs(string typeName)
        {
            TypeName = Preconditions.NotEmpty(typeName, nameof(typeName));
        }
        public string TypeName { get; }
    }
}