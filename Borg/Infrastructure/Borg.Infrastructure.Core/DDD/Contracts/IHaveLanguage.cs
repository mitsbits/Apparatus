namespace Borg.Infrastructure.Core.DDD.Contracts
{
    public interface IHaveLanguage<TKey, out TLanguage> where TLanguage : ILocalizationSilo
    {
        TKey LanguageID { get; }
        TLanguage Language { get; }
    }

    public interface IHAveJsonContent
    {
        public string JsonContent { get; }
    }
}