using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace eLog.HeavyTools.Services.WhWebShop.Service.Handlers
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly ILoginService loginService;

        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            ILoginService loginService) : base(options, logger, encoder, clock)
        {
            this.loginService = loginService ?? throw new ArgumentNullException(nameof(loginService));
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            this.Response.Headers.Add("WWW-Authenticate", "Basic");

            if (!this.Request.Headers.ContainsKey("Authorization"))
            {
                return AuthenticateResult.Fail("Authorization header missing");
            }

            // Get authorization key
            var authorizationHeader = this.Request.Headers["Authorization"].ToString();
            var authHeaderRegex = new System.Text.RegularExpressions.Regex(@"Basic (.*)");

            if (!authHeaderRegex.IsMatch(authorizationHeader))
            {
                return AuthenticateResult.Fail("Authorization code not formatted properly");
            }

            var authBase64 = Encoding.UTF8.GetString(Convert.FromBase64String(authHeaderRegex.Replace(authorizationHeader, "$1")));
            var authSplit = authBase64.Split(new[] { ':' }, 2);
            var authUsername = authSplit[0];
            var authPassword = authSplit.Length > 1 ? authSplit[1] : throw new Exception("Unable to get password");

            if (!await this.loginService.LoginAsync(authUsername, authPassword))
            {
                return AuthenticateResult.Fail("The username or password is not correct");
            }

            var identity = new ClaimsIdentity(authUsername);
            var claimsPrincipal = new ClaimsPrincipal(identity);

            return AuthenticateResult.Success(new AuthenticationTicket(claimsPrincipal, this.Scheme.Name));
        }
    }
}
