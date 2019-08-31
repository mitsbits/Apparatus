using Borg.Infrastructure.Core;
using Borg.Infrastructure.Core.DDD.Contracts;
using Borg.Infrastructure.Core.Services.Multilingual;
using System.Collections.Generic;
using System.Linq;
using static Borg.Framework.Services.Multilingual.LanguageOptionsProvider;

namespace Borg
{
    public static partial class ILanguageOptionsProviderExtensions
    {
        public static IEnumerable<IGlobalizationSilo> LanguagesEmptyAtTheBegining(this ILanguageOptionsProvider provider)
        {
            provider = Preconditions.NotNull(provider, nameof(provider));
            return new Language[] { Language.Enpty() }.Union(provider.Languages());
        }
    }
}