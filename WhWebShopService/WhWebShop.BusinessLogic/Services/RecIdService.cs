using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Dto;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model;
using eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Helpers;
using eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services.Base;
using eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services.Interfaces;
using eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Validators.Interfaces;
using eLog.HeavyTools.Services.WhWebShop.DataAccess.Repositories.Interfaces;
using FluentValidation;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;


namespace eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Services;

[RegisterDI(Interface = typeof(IRecIdService))]
public class RecIdService : LogicServiceBase<OlsRecid>, IRecIdService
{

    public RecIdService(IValidator<OlsRecid> validator, IRepository<OlsRecid> repository, IUnitOfWork unitOfWork, IEnvironmentService environmentService) : base(validator, repository, unitOfWork, environmentService)
    {
      
    }

    public async Task<OlsRecid?> GetCurrentAsync(string riid, CancellationToken cancellationtoken)
    {
        var e = await this.Query(p => p.Riid == riid).FirstOrDefaultAsync(cancellationtoken);

        return e;
    }

    public async Task<OlsRecid?> GetNewIdAsync(string riid, CancellationToken cancellationtoken)
    { /*
        var sqlParams = new SqlParameter[] {
            new SqlParameter("id", riid),
            new SqlParameter("store",  int.Parse("1")),
            new SqlParameter("createIfNotExists",  int.Parse("0")),
            new SqlParameter("idcount",  int.Parse("1"))
        };

        var sql = "sp_ols_getnewid " + String.Join(", ", sqlParams.Select(x =>
         $"@{x.ParameterName} = @{x.ParameterName}" +
         (x.Direction == ParameterDirection.Output ? " OUT" : "")
         ));
        await repository?.GetDatabase()?.ExecuteSqlRawAsync(sql, sqlParams)!;
        */

        await Repository.ExecuteStoredProcedure("sp_ols_getnewid", new Dictionary<string, object>
        {
            {"id", riid },
            {"store",  int.Parse("1") },
            {"createIfNotExists",  int.Parse("0") },
            {"idcount",  int.Parse("1") }
        });


       var e = await this.Query(p => p.Riid == riid).FirstOrDefaultAsync(cancellationtoken);
        
        return e;
    }
}
