using Borg.Infrastructure.Core.DDD;
using System;
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