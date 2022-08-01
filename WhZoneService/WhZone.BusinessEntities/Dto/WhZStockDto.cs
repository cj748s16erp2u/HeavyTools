using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Dto.Interfaces;

namespace eLog.HeavyTools.Services.WhZone.BusinessEntities.Dto;

public class WhZStockDto : WhZStockKey
{
    public decimal? Qty { get; set; }
}
