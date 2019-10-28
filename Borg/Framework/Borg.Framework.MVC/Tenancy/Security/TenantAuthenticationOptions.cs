using Borg.Infrastructure.Core;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Borg.Framework.MVC.Tenancy.Security
{
    public class TenantAuthenticationOptions : AuthenticationSchemeOptions
    {
        public string Realm { get; set; }
    }

    //TODO: services.AddSingleton<IPostConfigureOptions<TenantAuthenticationOption>, TenantAuthenticationPostConfigureOptions>();
    public class TenantAuthenticationPostConfigureOptions : IPostConfigureOptions<TenantAuthenticationOptions>
    {
        public void PostConfigure(string name, TenantAuthenticationOptions options)
        {
            if (string.IsNullOrEmpty(options.Realm))
            {
                throw new InvalidOperationException("Realm must be provided in options");
            }
        }
    }

    public class TenantAuthenticationHandler : AuthenticationHandler<TenantAuthenticationOptions>
    {
        private readonly ITenantAuthenticationService tenantAuthenticationService;

        public TenantAuthenticationHandler(
            IOptionsMonitor<TenantAuthenticationOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            ITenantAuthenticationService tenantAuthenticationService) : base(options, logger, encoder, clock)
        {
            this.tenantAuthenticationService = Preconditions.NotNull(tenantAuthenticationService, nameof(tenantAuthenticationService));
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey(TenantSecurityConstants.AuthorizationHeaderName))
            {
                //Authorization header not in request
                return AuthenticateResult.NoResult();
            }
            if (!AuthenticationHeaderValue.TryParse(Request.Headers[TenantSecurityConstants.AuthorizationHeaderName], out AuthenticationHeaderValue headerValue))
            {
                //Invalid Authorization header
                return AuthenticateResult.NoResult();
            }
            if (!TenantSecurityConstants.TenantSchemeName.Equals(headerValue.Scheme, StringComparison.OrdinalIgnoreCase))
            {
                //Not Basic authentication header
                return AuthenticateResult.NoResult();
            }

            byte[] headerValueBytes = Convert.FromBase64String(headerValue.Parameter);
            string userAndPassword = Encoding.UTF8.GetString(headerValueBytes);
            string[] parts = userAndPassword.Split(':');
            if (parts.Length != 2)
            {
                return AuthenticateResult.Fail("Invalid Basic authentication header");
            }
            string user = parts[0];
            string password = parts[1];
            bool isValidUser = await tenantAuthenticationService.IsValidUserAsync(user, password);

            if (!isValidUser)
            {
                return AuthenticateResult.Fail("Invalid username or password");
            }
            var claims = new[] { new Claim(ClaimTypes.Name, user) };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);
            return AuthenticateResult.Success(ticket);
        }

        protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            Response.Headers["WWW-Authenticate"] = $"{TenantSecurityConstants.TenantSchemeName} realm=\"{Options.Realm}\", charset=\"UTF-8\"";
            await base.HandleChallengeAsync(properties);
        }

        public interface ITenantAuthenticationService
        {
            Task<bool> IsValidUserAsync(string user, string password);
        }
    }
}