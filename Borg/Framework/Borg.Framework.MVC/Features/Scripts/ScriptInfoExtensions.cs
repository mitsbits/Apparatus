﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yahoo.Yui.Compressor;

namespace Borg.Framework.MVC.Features.Scripts
{
    internal static class ScriptInfoExtensions
    {

        private static readonly JavaScriptCompressor _jsCompressor = new JavaScriptCompressor() { CompressionType = CompressionType.Standard, DisableOptimizations = false, Encoding = Encoding.UTF8, ObfuscateJavascript = true, PreserveAllSemicolons = false };
        private static readonly CssCompressor _cssCompressor = new CssCompressor() { CompressionType = CompressionType.Standard, RemoveComments = true };
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
            if (!info.InlineContent.IsNullOrWhiteSpace())
            { 
                builder.AppendLine(_jsCompressor.Compress(info.InlineContent.Value));
            }
            builder.AppendLine("</script>");
            return builder.ToString();
        }

        public static string CssToHtml(ScriptInfo info)
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
                builder.AppendLine(_cssCompressor.Compress(info.InlineContent.Value));
                builder.AppendLine("</style>");
                return builder.ToString();
            }
        }

        public static string BundleCssToHtml(this IEnumerable<ScriptInfo> infos)
        {
            if (infos == null || !infos.Any() || !infos.All(x => x.InfoType == ScriptInfoType.Css)) return string.Empty;


            var builder = new StringBuilder("<style borg>");
            var innerBuilder = new StringBuilder();
            innerBuilder.AppendJoin(' ', infos.Where(x => !x.InlineContent.IsNullOrWhiteSpace()).Select(x => x.InlineContent));
            builder.AppendLine(_cssCompressor.Compress(innerBuilder.ToString()));
            builder.AppendLine("</style>");
            return builder.ToString();
        }
        public static string BundleJsToHtml(this IEnumerable<ScriptInfo> infos)
        {
            if (infos == null || !infos.Any() || !infos.All(x => x.InfoType == ScriptInfoType.Js)) return string.Empty;


            
            var builder = new StringBuilder("<script borg>");
            var innerbuilder = new StringBuilder();
            innerbuilder.AppendJoin(' ', infos.Where(x => !x.InlineContent.IsNullOrWhiteSpace()).Select(x=>x.InlineContent));
            var comporessor = 
            builder.AppendLine(_jsCompressor.Compress(innerbuilder.ToString()));
            builder.AppendLine("</script>");
            return builder.ToString();
        }
    }
}
