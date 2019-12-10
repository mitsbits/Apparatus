namespace Borg.Infrastructure.Core.DDD.Contracts
{
    public interface ILocalizationSilo
    {
        string TwoLetterISO { get; }
        string CultureName { get; }
    }
}