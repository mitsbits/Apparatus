using Borg.Infrastructure.Core;
using Borg.Framework.Dispatch;

namespace Borg.Framework.SQLServer.Broadcast.Migration
{
    public class RunMigrationCommand : IRequest
    {
        public RunMigrationCommand(int currnetSchemaVersion)
        {
            CurrnetSchemaVersion = Preconditions.Positive(currnetSchemaVersion, nameof(currnetSchemaVersion));
        }

        public int CurrnetSchemaVersion { get; }
    }
}