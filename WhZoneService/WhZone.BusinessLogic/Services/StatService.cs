using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Model;
using eLog.HeavyTools.Services.WhZone.BusinessLogic.Helpers;
using eLog.HeavyTools.Services.WhZone.BusinessLogic.Services.Interfaces;
using eLog.HeavyTools.Services.WhZone.DataAccess.Context;
using Microsoft.EntityFrameworkCore;

namespace eLog.HeavyTools.Services.WhZone.BusinessLogic.Services;

[RegisterDI(Interface = typeof(IStatService))]
internal class StatService : IStatService
{
    private readonly WhZoneDbContext dbContext;

    public StatService(
        WhZoneDbContext dbContext)
    {
        this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task<OlsStatline?> GetStatusValueAsync(string statKey, int value, CancellationToken cancellationToken = default)
    {
        var sql = $@"select top 1 [stl].*
from [ols_stathead] [sth] (nolock)
  join [ols_statline] [stl] (nolock) on [stl].[statgrpid] = [sth].[statgrpid]
where [sth].[statkey] like {Utils.ToSqlValue(statKey)}
  and [stl].[value] = {Utils.ToSqlValue(value)}
";

        var queryString = FormattableStringFactory.Create(sql);

        return await this.dbContext.Set<OlsStatline>().FromSqlInterpolated(queryString).FirstOrDefaultAsync(cancellationToken);
    }
}
