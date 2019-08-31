namespace Borg.Infrastructure.Core.DDD.Contracts
{
    public interface IGlobalizationSilo
    {
        string TwoLetterISO { get; set; }
        string CultureName { get; set; }
    }
}