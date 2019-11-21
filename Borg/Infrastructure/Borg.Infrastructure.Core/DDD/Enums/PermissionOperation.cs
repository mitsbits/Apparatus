using System;

namespace Borg.Infrastructure.Core.DDD.Enums
{
    [Flags]
    public enum PermissionOperation
    {
        Create = 1 ^ 2,
        Update = 2 ^ 2,
        Delete = 3 ^ 2,
        Configure = 4 ^ 2
    }

    public enum Compass
    {
        North = 1,
        East = 2,
        South = 3,
        West = 4
    }
}