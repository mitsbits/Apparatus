using Borg.Infrastructure.Core;
using Borg.Infrastructure.Core.Reflection.Discovery;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Borg.Framework.ApplicationSettings
{
   public class ApplicationSettingsAssemblyScanResult : AssemblyScanResult
    {
        public ApplicationSettingsAssemblyScanResult(Assembly assembly, List<Type> applicationSettingTypes) : base(assembly, true, new string[0])
        {
            ApplicationSettingTypes = Preconditions.NotEmpty(applicationSettingTypes, nameof(applicationSettingTypes));
        }

        public ApplicationSettingsAssemblyScanResult(Assembly assembly, string[] errors) : base(assembly, false, errors)
        {
            ApplicationSettingTypes = null;
        }
        public IEnumerable<Type> ApplicationSettingTypes { get; }
    }
}
