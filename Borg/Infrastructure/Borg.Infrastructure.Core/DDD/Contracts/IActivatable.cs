using System;

namespace Borg.Infrastructure.Core.DDD.Contracts
{
    public interface IActive
    {
        bool IsActive { get; }
    }

    public interface IActivatable : IActive
    {
        DateTimeOffset? ActiveFrom { get; }
        DateTimeOffset? ActiveTo { get; }
        string ActivationID { get; }
        string DeActivationID { get; }
    }
}