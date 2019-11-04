using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Linq;
using System.Threading.Tasks;

namespace Borg.Framework.MVC.Features.Scripts
{
    [HtmlTargetElement("style", Attributes = "borg")]
    public class CssTagHelper : ScriptTagHelperBase
    {
        public CssTagHelper(IScriptStore store) : base(store)
        {
        }

        [HtmlAttributeName("borg-key")]
        public string BorgKey { get; set; }
        [HtmlAttributeName("borg-position")]
        public override ScriptPosition BorgScriptPosition { get; set; } = ScriptPosition.HeadBeforeEnd;

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var key = output.Attributes["key"];
            var hasKey = key != null;

            var src = output.Attributes["src"];
            var hasSrc = src != null;

            var info = new CssInfo
            {
                Key = hasKey ? key.Value.ToString() : string.Empty,
                Src = hasSrc ? src.Value.ToString() : string.Empty
            };
            if (info.Src.IsNullOrWhiteSpace())
            {
                info.InlineContent = new HtmlString((await output.GetChildContentAsync()).GetContent());
            }

            info.Position = BorgScriptPosition;

            if (info.Key.IsNullOrWhiteSpace())
            {
                await store.Add(info);
            }
            else
            {
                await store.AddOnce(info);
            }

            output.SuppressOutput();
        }
    }

    [HtmlTargetElement("style-render", Attributes = "borg")]
    public class CssRenderTagHelper : ScriptTagHelperBase
    {
        public CssRenderTagHelper(IScriptStore store) : base(store)
        {
        }
        [HtmlAttributeName("borg-position")]
        public override ScriptPosition BorgScriptPosition { get; set; } =ScriptPosition.BodyBeforeEnd;

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var infos = await store.GetForPosition(ScriptInfoType.Css, BorgScriptPosition);
            output.TagName = null;
            if (!infos.Any())
            {
                output.SuppressOutput();
                return;
            }
            foreach (var info in infos)
            {
                output.PostContent.AppendHtmlLine(info.ToHtml());
            }
        }
    }
}