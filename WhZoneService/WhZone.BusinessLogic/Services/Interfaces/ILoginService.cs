using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Services.WhZone.BusinessLogic.Services.Interfaces;

public interface ILoginService
{
    ValueTask<string?> GetUserIDAsync(string? userID, CancellationToken cancellationToken = default);
    ValueTask<bool> LoginAsync(string? userID, string? password, CancellationToken cancellationToken = default);
}
