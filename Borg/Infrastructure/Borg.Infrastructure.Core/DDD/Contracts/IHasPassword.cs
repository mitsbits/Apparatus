namespace Borg.Infrastructure.Core.DDD.Contracts
{
    public interface IHavePassword
    {
        string PasswordHash { get; set; }
    }
}