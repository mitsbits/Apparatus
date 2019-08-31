namespace Borg.Infrastructure.Core.DDD.Contracts
{
    public interface IHaveLanguage<TKey, out TLanguage> where TLanguage : IGlobalizationSilo
    {
        TKey LanguageID { get; }
        TLanguage Language { get; }
    }
}