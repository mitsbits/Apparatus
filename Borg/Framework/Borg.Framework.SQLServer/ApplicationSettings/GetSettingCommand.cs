using Borg.Framework.ApplicationSettings;
using Borg.Infrastructure.Core;
using MediatR;
using System;

namespace Borg.Framework.SQLServer.ApplicationSettings
{
    public class GetSettingCommand : IRequest<PaylodCommandResult>
    {
        public GetSettingCommand(Type settingType)
        {
            var type = Preconditions.NotNull(settingType, nameof(settingType));
            if (!type.ImplementsInterface<IApplicationSetting>())
            {
                throw new ArgumentException($"{GetType().Name} does not implement mandatory {nameof(IApplicationSetting)}");
            }
            SettingType = type;
        }

        public Type SettingType { get; }
    }
}