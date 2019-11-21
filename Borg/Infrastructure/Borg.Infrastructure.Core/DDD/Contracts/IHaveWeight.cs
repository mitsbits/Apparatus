namespace Borg.Infrastructure.Core.DDD.Contracts
{
    public interface IHaveWeight
    {
        double Weight { get; }
    }
    public interface IHaveWidth
    {
        int Width { get; }
    }

    public interface IHaveHeight
    {
        int Height { get; }
    }
}