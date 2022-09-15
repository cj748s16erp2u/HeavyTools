using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Enums;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model;
using eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Helpers;
using eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services.Interfaces;
using eLog.HeavyTools.Services.WhWebShop.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services;

[RegisterDI(Interface = typeof(ILoginService))]
internal class LoginService : ILoginService
{
    private readonly IRepository<CfwUser> userRepository;
    private readonly IOlsSysvalService sysvalService;
    private readonly IOptions<Options.CryptoOptions> cryptoOptions;

    public LoginService(
        IRepository<CfwUser> userRepository,
        IOlsSysvalService sysvalService,
        IOptions<Options.CryptoOptions> cryptoOptions)
    {
        this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        this.sysvalService = sysvalService ?? throw new ArgumentNullException(nameof(sysvalService));
        this.cryptoOptions = cryptoOptions ?? throw new ArgumentNullException(nameof(cryptoOptions));
    }

    public async ValueTask<bool> LoginAsync(string? userID, string? password, CancellationToken cancellationToken = default)
    {
        ValidateLoginParameters(userID, password);

        var user = await this.userRepository.Entities
            .Include(e => e.CfwUsergroup)
            .Select(u => new { u.Usrid, u.Options, u.Password, grpExists = u.CfwUsergroup.Any() })
            .FirstOrDefaultAsync(u => u.Usrid == userID!, cancellationToken);
        if (user is null)
        {
            return false;
        }

        var options = (UserOption)user.Options;
        if ((options & UserOption.AccountDisabled) == UserOption.AccountDisabled)
        {
            return false;
        }

        if ((options & UserOption.AccountLockedOut) == UserOption.AccountLockedOut)
        {
            return false;
        }

        if (!user.grpExists)
        {
            return false;
        }

        var pwd = MD5(userID!, password!);
        var result = pwd != null && pwd.Equals(user.Password);
        if (!result)
        {
            var sysval = await this.sysvalService.GetAsync("eprojectweb.gpd", cancellationToken);
            if (sysval is not null && sysval.Valuevar is string gpd && !string.IsNullOrWhiteSpace(gpd))
            {
                var dgpd = DecryptGeneralPassword(gpd);
                var gPwd = MD5(userID!, dgpd!);
                result = pwd != null && pwd.Equals(gPwd);
            }
        }

        return result;
    }

    public async ValueTask<string?> GetUserIDAsync(string? userID, CancellationToken cancellationToken = default)
    {
        ValidateGetUserIDParameters(userID);

        var user = await this.userRepository.Entities.FirstOrDefaultAsync(u => u.Usrid == userID!, cancellationToken);
        return user?.Usrid;
    }

    private System.Security.Cryptography.RSACryptoServiceProvider? CreateRSAProv()
    {
        if (!string.IsNullOrWhiteSpace(this.cryptoOptions.Value.RSAKey))
        {
#pragma warning disable CA1416 // Validate platform compatibility
            var cspParams = new System.Security.Cryptography.CspParameters
            {
                Flags = System.Security.Cryptography.CspProviderFlags.UseMachineKeyStore
            };

#pragma warning disable S4426 // Cryptographic keys should be robust
            var prov = new System.Security.Cryptography.RSACryptoServiceProvider(cspParams);
#pragma warning restore S4426 // Cryptographic keys should be robust
#pragma warning restore CA1416 // Validate platform compatibility

            prov.FromXmlString(this.cryptoOptions.Value.RSAKey);

            return prov;
        }

        return null;
    }

    private string? DecryptGeneralPassword(string? password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            return null;
        }

        var prov = this.CreateRSAProv();
        if (prov is null)
        {
            return null;
        }

        return Encoding.UTF8.GetString(prov.Decrypt(HexToBytes(password), false));
    }

    private static byte[] HexToBytes(string input)
    {
        if (string.IsNullOrWhiteSpace(input) || input.Length == 0)
        {
            return new byte[] { 0 };
        }

        if (input.Length % 2 == 1)
        {
            input = "0" + input;
        }

        var result = new byte[input.Length / 2];
        for (int i = 0, count = input.Length / 2; i < count; i++)
        {
            result[i] = byte.Parse(input.Substring(i * 2, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
        }

        return result;
    }

    private static string MD5(string userId, string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            password = string.Empty;
        }

        var bs = Encoding.UTF8.GetBytes($"{userId.ToLowerInvariant()}\\cal2009auth\\{password}");
        var prov = System.Security.Cryptography.MD5.Create();
        bs = prov.ComputeHash(bs);
        return string.Join("", bs.Select(x => x.ToString("x2")));
    }

    private static void ValidateLoginParameters(string? userID, string? password)
    {
        if (string.IsNullOrWhiteSpace(userID))
        {
            throw new ArgumentNullException(nameof(userID));
        }

        if (string.IsNullOrWhiteSpace(password))
        {
            throw new ArgumentNullException(nameof(password));
        }
    }

    private static void ValidateGetUserIDParameters(string? userID)
    {
        if (string.IsNullOrWhiteSpace(userID))
        {
            throw new ArgumentNullException(nameof(userID));
        }
    }
}
