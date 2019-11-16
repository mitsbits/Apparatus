using Borg.Infrastructure.Core.DDD;
using Borg.Infrastructure.Core.DDD.ValueObjects;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Borg.Framework.ApplicationSettings
{
    public abstract class ApplicationSettingBase : ValueObject<ApplicationSettingBase>, IApplicationSetting
    {
        protected ApplicationSettingBase()
        {
            Title = ResolveTitle();
        }

        public string Title { get; }

        public string Key => GetType().FullName;

        public CompositeKey Keys => new CompositeKey(new KeyValuePair<string, object>[] { new KeyValuePair<string, object>(nameof(Key), Key) });

        private string ResolveTitle()
        {
            var attr = GetType().GetCustomAttribute<ApllicationSettingTitleAttribute>();
            if (attr != null)
            {
                return attr.Title.SplitUpperCaseToWords();
            }
            else
            {
                var typeName = GetType().Name;

                if (typeName.EndsWith("settings", StringComparison.InvariantCultureIgnoreCase))
                {
                    typeName = typeName.Substring(0, typeName.IndexOf("settings", StringComparison.InvariantCultureIgnoreCase));
                }
                return typeName.SplitUpperCaseToWords();
            }
        }
    }
}