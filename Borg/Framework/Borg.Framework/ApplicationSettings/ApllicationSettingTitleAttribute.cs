using Borg.Infrastructure.Core;
using Borg.Infrastructure.Core.DDD.Contracts;
using System;

namespace Borg.Framework.ApplicationSettings
{
    public class ApllicationSettingTitleAttribute : Attribute, IHaveTitle
    {
        public ApllicationSettingTitleAttribute(string title)
        {
            Title = Preconditions.NotEmpty(title, nameof(title));
        }

        public string Title { get; }
    }
}