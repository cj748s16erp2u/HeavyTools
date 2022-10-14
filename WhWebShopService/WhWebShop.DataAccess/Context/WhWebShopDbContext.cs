using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;

namespace eLog.HeavyTools.Services.WhWebShop.DataAccess.Context
{
    public partial class WhWebShopDbContext : DbContext
    {
        public virtual DbSet<OlcPriceCalcResult> OlcPriceCalcResults { get; set; } = null!;


        public virtual DbSet<SordReserveRecalcTmp> SordReserveRecalcTmp { get; set; } = null!;
    }
}
