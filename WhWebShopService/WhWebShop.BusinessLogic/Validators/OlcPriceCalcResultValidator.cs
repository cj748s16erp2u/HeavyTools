using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model;
using eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Helpers;
using eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Validators.Base;
using eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Validators.Interfaces;
using eLog.HeavyTools.Services.WhWebShop.DataAccess.Context;

namespace eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Validators;

[RegisterDI(Interface = typeof(IOlcPriceCalcResultValidator), Lifetime = Microsoft.Extensions.DependencyInjection.ServiceLifetime.Transient)]
internal class OlcPriceCalcResultValidator : EntityValidator<OlcPriceCalcResult>, IOlcPriceCalcResultValidator
{
    public OlcPriceCalcResultValidator(WhWebShopDbContext dbContext) : base(dbContext)
    {
    }
}
