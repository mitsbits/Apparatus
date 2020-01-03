using System.Threading.Tasks;

namespace Borg.Framework.Services.Multilingual
{
    internal interface ITranslationService
    {
        ValueTask<string> GetTranslation(int tenantId, int languageId, string key, string defaultValue);

        ValueTask<string> GetTranslation(int tenantId, string languageIsoCode, string key, string defaultValue);
    }
}