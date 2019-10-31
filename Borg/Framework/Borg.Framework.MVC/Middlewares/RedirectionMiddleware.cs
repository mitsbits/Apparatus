using Borg.Framework.Redirections;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using System.Threading.Tasks;

namespace Borg.Framework.MVC.Middlewares
{
    public class RedirectionMiddleware
    {
        private readonly RequestDelegate _next;

        public RedirectionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IRedirectEvaluator evaluator)
        {
            var httpRequestFeature = context.Request.HttpContext.Features.Get<IHttpRequestFeature>();
            var redirection = await evaluator.Evaluate(httpRequestFeature.RawTarget);
            if (redirection.ShouldRedirect)
            {
                context.Response.Redirect(redirection.RedirectTo, redirection.StatusCode == RedirectStatusCode.Permanent);
            }

            await _next.Invoke(context);

            // Clean up.
        }
    }
}