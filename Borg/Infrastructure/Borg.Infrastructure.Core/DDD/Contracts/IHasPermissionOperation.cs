using Borg.Infrastructure.Core.DDD.Enums;

namespace Borg.Infrastructure.Core.DDD.Contracts
{
    public interface IHavePermissionOperation
    {
        PermissionOperation PermissionOperation { get; set; }
    }
}