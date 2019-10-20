using Borg.Framework.ApplicationSettings;
using Borg.Infrastructure.Core;
using MediatR;
using System;

namespace Borg.Framework.SQLServer.ApplicationSettings
{
    public class UpdateOrCreateCommand : IRequest<PaylodCommandResult>
    {
        public UpdateOrCreateCommand(object payload, Type settingType)
        {
            var type = Preconditions.NotNull(settingType, nameof(settingType));
            if (!type.ImplementsInterface<IApplicationSetting>())
            {
                throw new ArgumentException($"{GetType().Name} does not implement mandatory {nameof(IApplicationSetting)}");
            }
            SettingType = type;
            Payload = Preconditions.NotNull(payload, nameof(payload));
        }

        public object Payload { get; }
        public Type SettingType { get; }
    }
}