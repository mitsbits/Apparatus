using MediatR;

namespace Borg.Framework.SQLServer.ApplicationSettings
{
    public class GetSettingsCommand : IRequest<PaylodCommandResult[]>
    {
        public GetSettingsCommand()
        {

        }

    }
}