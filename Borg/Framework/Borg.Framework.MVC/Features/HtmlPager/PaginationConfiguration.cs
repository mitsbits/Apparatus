using System;
using System.ComponentModel;

namespace Borg.Framework.MVC.Features.HtmlPager
{
    [Serializable]
    public class PaginationConfiguration
    {
        [DefaultValue("ul")]
        public virtual string OutputTagElement { get; set; } = "ul";

        [DefaultValue("li")]
        public virtual string OutputItemTagElement { get; set; } = "li";

        [DefaultValue("{0} to {1} of {2}")]
        public virtual string ItemSliceAndTotalFormat { get; set; } = "{0} to {1} of {2}";

        [DefaultValue("{0} of {1}")]
        public virtual string PageCountAndLocationFormat { get; set; } = "{0} of {1}";

        [DefaultValue(">")]
        public virtual string NextDisplay { get; set; } = ">";

        [DefaultValue(">>")]
        public virtual string LastDisplay { get; set; } = ">>";

        [DefaultValue("<")]
        public virtual string PreviousDisplay { get; set; } = "<";

        [DefaultValue("<<")]
        public virtual string FirstDisplay { get; set; } = "<<";

        [DefaultValue("{0}")]
        public virtual string PageDisplayFormat { get; set; } = "{0}";

        [DefaultValue("page-link")]
        public virtual string LinkClass { get; set; } = "page-link";

        [DefaultValue("page-item")]
        public virtual string ItemClass { get; set; } = "page-item";

        [DefaultValue("pagination")]
        public virtual string ElementClass { get; set; } = "pagination";

        [DefaultValue("current")]
        public virtual string CurrentClass { get; set; } = "current";

        [DefaultValue("unavailable")]
        public virtual string UnavailableClass { get; set; } = "unavailable";

        [DefaultValue("arrow")]
        public virtual string ArrowClass { get; set; } = "arrow";

        [DefaultValue("...")]
        public string Ellipses { get; set; } = "...";

        public PaginationConfigurationBehaviour Behaviour { get; set; } = new PaginationConfigurationBehaviour();
    }
}