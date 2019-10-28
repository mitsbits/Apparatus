using System;

namespace Borg.Framework.Redirections
{
    public struct RedirectEvaluation
    {
        private static readonly Lazy<RedirectEvaluation> negative = new Lazy<RedirectEvaluation>(() =>
            new RedirectEvaluation(string.Empty, false, RedirectStatusCode.Undefined));

        public RedirectEvaluation(string redirectTo, RedirectStatusCode statusCode) : this(redirectTo, true, statusCode)
        {
        }

        private RedirectEvaluation(string redirectTo, bool shouldRedirect, RedirectStatusCode statusCode)
        {
            RedirectTo = redirectTo;
            ShouldRedirect = shouldRedirect;
            StatusCode = statusCode;
        }

        public static RedirectEvaluation Negative => negative.Value;

        public bool ShouldRedirect { get; internal set; }
        public string RedirectTo { get; internal set; }
        public RedirectStatusCode StatusCode { get; internal set; }
    }
}