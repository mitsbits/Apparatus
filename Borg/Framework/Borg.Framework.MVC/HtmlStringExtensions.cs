using Microsoft.AspNetCore.Html;

namespace Borg
{
    public static class HtmlStringExtensions
    {
        public static bool IsNullOrWhiteSpace(this HtmlString source)
        {
            if (source == null || source.Value == null) return true;
            return source.Value.IsNullOrWhiteSpace();
        }
    }
}