using System.Text;

namespace Borg.Framework.MVC.Features.Scripts
{
    internal static class ScriptInfoExtensions
    {
        internal static string ToHtml(this ScriptInfo info)
        {
            if (info.InfoType == ScriptInfoType.Js)
            {
                return JsToHtml(info);
            }
            if (info.InfoType == ScriptInfoType.Css)
            {
                return CssToHtml(info);
            }
            return string.Empty;
        }

        private static string JsToHtml(ScriptInfo info)
        {
            var builder = new StringBuilder("<script ");
            if (!info.Key.IsNullOrWhiteSpace())
            {
                builder.Append($" borg-key='{info.Key}'");
            }
            else
            {
                builder.Append($" borg");
            }
            if (!info.Src.IsNullOrWhiteSpace())
            {
                builder.Append($" src='{info.Src}'");
            }
            builder.Append(" >");
            if (!info.InlineContent.Value.IsNullOrWhiteSpace())
            {
                builder.AppendLine(info.InlineContent.Value);
            }
            builder.AppendLine("</script>");
            return builder.ToString();
        }

        private static string CssToHtml(ScriptInfo info)
        {
            var cssFileLink = !info.Src.IsNullOrWhiteSpace();
            if (cssFileLink)
            {
                var builder = new StringBuilder("<link rel='stylesheet' ");
                if (!info.Key.IsNullOrWhiteSpace())
                {
                    builder.Append($" borg-key='{info.Key}'");
                }
                else
                {
                    builder.Append($" borg");
                }
                builder.Append($" href='{info.Src}' />");
                return builder.ToString();
            }
            else
            {
                var builder = new StringBuilder("<style>");
                builder.AppendLine(info.InlineContent.Value);
                builder.AppendLine("</style>");
                return builder.ToString();
            }
        }
    }
}