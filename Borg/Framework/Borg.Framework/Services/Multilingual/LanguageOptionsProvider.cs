using Borg.Infrastructure.Core;
using Borg.Infrastructure.Core.DDD;
using Borg.Infrastructure.Core.DDD.Contracts;
using Borg.Infrastructure.Core.DI;
using Borg.Infrastructure.Core.Services.Multilingual;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Borg.Framework.Services.Multilingual
{
    [PlugableService(ImplementationOf = typeof(ILanguageOptionsProvider), Lifetime = Lifetime.Singleton, OneOfMany = true, Order = -999)]
    public class LanguageOptionsProvider : ILanguageOptionsProvider
    {
        private readonly ConcurrentDictionary<string, string> source;

        public LanguageOptionsProvider()
        {
            var cultureInfos = CultureInfo.GetCultures(CultureTypes.AllCultures);
            source = new ConcurrentDictionary<string, string>(cultureInfos.Select(x => new KeyValuePair<string, string>(x.TwoLetterISOLanguageName, x.Name)));
        }

        public IEnumerable<IGlobalizationSilo> Languages()
        {
            return source.Select(x => new Language(x.Key, x.Value));
        }

        internal class Language : ValueObject<Language>, IGlobalizationSilo
        {
            private readonly string key;
            private readonly string title;

            public Language(string key, string title) : this(key, title, false)
            {
            }

            internal Language(string key, string title, bool allowEmpty)
            {
                if (!allowEmpty)
                {
                    this.key = Preconditions.NotEmpty(key, nameof(key));
                    this.title = Preconditions.NotEmpty(title, nameof(title));
                }
                else
                {
                    this.key = string.Empty;
                    this.title = string.Empty;
                }
                this.key = key;
                this.title = title;
            }

            internal static Language Enpty()
            {
                return new Language(string.Empty, string.Empty, true);
            }

            public string TwoLetterISO { get => key; set { throw new ApplicationException($"{nameof(TwoLetterISO)} is set at the constructor"); } }
            public string Title { get => title; set { throw new ApplicationException($"{nameof(Title)} is set at the constructor"); } }

            public string CultureName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        }
    }
}

namespace Borg
{
}