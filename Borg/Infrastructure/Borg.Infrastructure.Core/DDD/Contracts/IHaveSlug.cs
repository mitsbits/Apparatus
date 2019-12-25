namespace Borg.Infrastructure.Core.DDD.Contracts
{
    public interface IHaveSlug
    {
        string Slug { get; }
    }

    public interface IHaveFullSlug
    {
        string FullSlug { get; }
    }
}