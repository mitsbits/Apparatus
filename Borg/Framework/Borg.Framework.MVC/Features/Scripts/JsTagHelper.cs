using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Linq;
using System.Threading.Tasks;

namespace Borg.Framework.MVC.Features.Scripts
{
    [HtmlTargetElement("script", Attributes = "borg")]
    public class JsTagHelper : ScriptTagHelperBase
    {
        public JsTagHelper(IScriptStore store) : base(store)
        {
        }

        [HtmlAttributeName("borg-key")]
        public string BorgKey { get; set; }
        [HtmlAttributeName("borg-position")]
        public override ScriptPosition BorgScriptPosition { get; set; } = ScriptPosition.BodyBeforeEnd;
        [HtmlAttributeName("src")]
        public string Src { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
 
            var hasKey = !BorgKey.IsNullOrWhiteSpace();

            var hasSrc = !Src.IsNullOrWhiteSpace();

            var info = new JsInfo
            {
                Key = hasKey ? BorgKey : string.Empty,
                Src = hasSrc ? Src.StartsWith("~/") ? Src.Replace("~/", "") : Src : string.Empty
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

    [HtmlTargetElement("script-render", Attributes = "borg")]
    public class JsRenderTagHelper : ScriptTagHelperBase
    {
        public JsRenderTagHelper(IScriptStore store) : base(store)
        {
        }
        [HtmlAttributeName("borg-position")]
        public override ScriptPosition BorgScriptPosition { get; set; } = ScriptPosition.BodyBeforeEnd;

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var infos = await store.GetForPosition(ScriptInfoType.Js, BorgScriptPosition);
            output.TagName = null;
            if (!infos.Any())
            {
                output.SuppressOutput();
                return;
            }
            foreach (var info in infos.Where(x => !x.Src.IsNullOrWhiteSpace()))
            {
                output.PostContent.AppendHtmlLine(info.ToHtml());
            }
            output.PostContent.AppendHtmlLine(infos.Where(x => !x.InlineContent.IsNullOrWhiteSpace()).BundleJsToHtml());
        }
    }
}