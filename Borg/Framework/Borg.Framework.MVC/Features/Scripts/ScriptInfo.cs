using Microsoft.AspNetCore.Html;
using System.Text;

namespace Borg.Framework.MVC.Features.Scripts
{
    public abstract class ScriptInfo
    {
        internal string Src { get; set; }
        internal string Key { get; set; }
        internal HtmlString InlineContent { get; set; }
        internal abstract ScriptPosition Position { get; set; }
        internal abstract ScriptInfoType InfoType { get; }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append($"[{InfoType.ToString()}]");
            if (!Key.IsNullOrWhiteSpace())
            {
                builder.Append($"[{Key}]");
            }
            builder.Append($"[{Position.ToString()}]");
            if (!string.IsNullOrWhiteSpace(Src))
            {
                builder.Append($"[src:{Src}]");
            }
            else
            {
                builder.Append($"[src:inline]");
            }
            return builder.ToString();
        }
    }

    public class JsInfo : ScriptInfo
    {
        internal override ScriptPosition Position { get; set; } = ScriptPosition.BodyBeforeEnd;
        internal override ScriptInfoType InfoType => ScriptInfoType.Js;
    }

    public class CssInfo : ScriptInfo
    {
        internal override ScriptPosition Position { get; set; } = ScriptPosition.HeadBeforeEnd;
        internal override ScriptInfoType InfoType => ScriptInfoType.Css;
    }
}