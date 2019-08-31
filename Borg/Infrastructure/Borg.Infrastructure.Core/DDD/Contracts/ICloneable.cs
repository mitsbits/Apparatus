namespace Borg.Infrastructure.Core.DDD.Contracts
{
    public interface ICloneable<out T> where T : class
    {
        T Clone();
    }
}