namespace Borg.Infrastructure.Core.DDD.Contracts
{
    public interface ITreeNode : IEntity
    {
        int? ParentId { get; }
        int Depth { get; }
        string Hierarchy { get; }
    }
}