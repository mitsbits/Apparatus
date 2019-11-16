using Borg.Infrastructure.Core;
using Borg.Infrastructure.Core.Reflection.Discovery;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Borg.Framework.ApplicationSettings
{
    public class ApplicationSettingTypeProvider : IApplicationSettingTypeProvider
    {
        private List<Type> _types = new List<Type>();
        public ApplicationSettingTypeProvider(IAssemblyExplorerResult explorerResult)
        {
            explorerResult = Preconditions.NotNull(explorerResult, nameof(explorerResult));
            var result = explorerResult.Results<ApplicationSettingsAssemblyScanResult>();
            _types.AddRange(result.SelectMany(x => x.ApplicationSettingTypes).Distinct());
        }

        public IEnumerator<Type> GetEnumerator() => _types.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}