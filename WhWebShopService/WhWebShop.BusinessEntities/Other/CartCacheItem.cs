using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Other;

public class CartCacheItem
{
    public CalcJsonResultDto CalcJsonResultDto { get; set; } = null!;
    public DateTime Expire { get; set; } = DateTime.Now.AddMinutes(30);
}
