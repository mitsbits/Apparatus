using Borg.Infrastructure.Core;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Borg.Framework.MVC.Features.Scripts
{
    public abstract class ScriptTagHelperBase : TagHelper
    {
        protected readonly IScriptStore store;

        protected ScriptTagHelperBase(IScriptStore store)
        {
            this.store = Preconditions.NotNull(store, nameof(store));
        }

        [HtmlAttributeName("borg-position")]
        public abstract ScriptPosition BorgScriptPosition { get; set; }
    }
}