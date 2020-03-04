using Borg.Infrastructure.Core.DDD.ValueObjects;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json.Serialization;

namespace Borg.Framework.ApplicationSettings
{
    public abstract class ApplicationSettingBase : IApplicationSetting
    {
        protected ApplicationSettingBase()
        {
            Title = ResolveTitle();
        }
        [JsonIgnore]
        public string Title { get; }
        [JsonIgnore]
        public string Key => GetType().FullName;
        [JsonIgnore]
        public CompositeKey Keys => new CompositeKey(new KeyValuePair<string, object>[] { new KeyValuePair<string, object>(nameof(Key), Key) });
        [JsonIgnore]
        public string Icon { get; set; } = "https://encrypted-tbn0.gstatic.com/images?q=tbn%3AANd9GcQ_bLeq1astXNFHP7H9gDt3kUXXahZnX8S_TMQw4wok8iLok2sJ";
        [JsonIgnore]
        public int Order { get; set; } = 0;
        [JsonIgnore]
        public string JsonContent { get; set; } = "{}";
        [JsonIgnore]
        public int TenantId { get; set; } = 0;
        [JsonIgnore]
        public int LanguageId { get; set; } = 0;


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