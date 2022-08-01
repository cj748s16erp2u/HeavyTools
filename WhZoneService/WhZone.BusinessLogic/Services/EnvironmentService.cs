using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.Services.WhZone.BusinessLogic.Helpers;
using eLog.HeavyTools.Services.WhZone.BusinessLogic.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace eLog.HeavyTools.Services.WhZone.BusinessLogic.Services;

[RegisterDI(Interface = typeof(IEnvironmentService))]
internal class EnvironmentService : IEnvironmentService
{
    /// <summary>
    /// The HTTP context accessor
    /// </summary>
    protected readonly IHttpContextAccessor httpContextAccessor;
    private readonly ILoginService loginService;

    ///// <summary>
    ///// The security token handler
    ///// </summary>
    //protected readonly JwtSecurityTokenHandler securityTokenHandler;

    ///// <summary>
    ///// Initializes a new instance of the <see cref="EnvironmentService"/> class.
    ///// </summary>
    //public EnvironmentService()
    //{
    //}

    /// <summary>
    /// Initializes a new instance of the <see cref="EnvironmentService"/> class.
    /// </summary>
    /// <param name="httpContextAccessor">The HTTP context accessor.</param>
    /// <exception cref="ArgumentNullException">httpContextAccessor</exception>
    public EnvironmentService(
        IHttpContextAccessor httpContextAccessor,
        ILoginService loginService)
    {
        this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        this.loginService = loginService ?? throw new ArgumentNullException(nameof(loginService));
        //this.securityTokenHandler = new JwtSecurityTokenHandler();
    }

    protected virtual string? Authorization
    {
        get
        {
            if (!this.httpContextAccessor.HttpContext?.Request?.Headers["Authorization"].Any() ?? true)
            {
                return null;
            }

            var authorization = this.httpContextAccessor.HttpContext!.Request.Headers["Authorization"].First();

            if (authorization.StartsWith("Negotiate", StringComparison.InvariantCultureIgnoreCase) ||
                authorization.StartsWith("NTLM", StringComparison.InvariantCultureIgnoreCase) /*||
                authorization.StartsWith("Basic", StringComparison.InvariantCultureIgnoreCase)*/||
                authorization.StartsWith("Bearer", StringComparison.InvariantCultureIgnoreCase))
            {
                return null;
            }

            return authorization;
        }
    }

    //protected JwtSecurityToken AuthToken
    protected System.Net.NetworkCredential? AuthToken
    {
        get
        {
            var authorization = this.Authorization;
            if (!string.IsNullOrWhiteSpace(authorization))
            {
                try
                {
                    //return this.securityTokenHandler.ReadToken(authorization.Replace("Bearer", string.Empty).Trim()) as JwtSecurityToken;
                    authorization = authorization.Replace("Basic", string.Empty).Trim();
                    byte[]? buf = null;
                    if (!string.IsNullOrWhiteSpace(authorization))
                    {
                        buf = Convert.FromBase64String(authorization);
                    }

                    string? up = null;
                    if (buf != null)
                    {
                        up = Encoding.GetEncoding("ISO-8859-1").GetString(buf);
                    }

                    if (!string.IsNullOrWhiteSpace(up))
                    {
                        var p = up.Split(new[] { ':' }, 2);
                        if (p.Length == 2)
                        {
                            return new System.Net.NetworkCredential(p[0], p[1]);
                        }
                    }
                }
                catch
                {
                    // hibas kulcs eseten nincs feldolgozas
                }
            }

            return null;
        }
    }

    /// <summary>
    /// Gets the current user identifier.
    /// </summary>
    /// <value>
    /// The current user identifier.
    /// </value>
    public virtual string? CurrentUserId
    {
        get
        {
            //var token = this.AuthToken;
            //if (token != null)
            //{
            //    var nameIdentifier = token.Claims.First(x => x.Type == ClaimTypes.NameIdentifier);
            //    return int.Parse(nameIdentifier.Value);
            //}
            var token = this.AuthToken;
            if (token is not null)
            {
                return this.loginService.GetUserIDAsync(token.UserName).Result;
            }

            return null;
        }
    }
}
