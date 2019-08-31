using Borg.Infrastructure.Core.DDD.Contracts;
using System.Collections.Generic;

namespace Borg.Infrastructure.Core.Services.Multilingual
{
    public interface ILanguageOptionsProvider
    {
        IEnumerable<IGlobalizationSilo> Languages();
    }
}