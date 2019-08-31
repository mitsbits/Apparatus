using Borg.Infrastructure.Core.DDD.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Borg.Infrastructure.Core.DDD.Contracts
{
   public interface IHavePermissionOperation
    {
        PermissionOperation PermissionOperation { get; set; }
    }
}
