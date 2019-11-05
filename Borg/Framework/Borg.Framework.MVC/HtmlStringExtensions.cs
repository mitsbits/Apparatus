using Microsoft.AspNetCore.Html;
using System;
using System.Collections.Generic;
using System.Text;

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
