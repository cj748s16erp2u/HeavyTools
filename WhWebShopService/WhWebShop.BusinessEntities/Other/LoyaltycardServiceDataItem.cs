using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Other;

public class LoyaltycardDataItem
{
    public decimal TotalPurchaseAmount { get; set; } = 0;

    public DateTime ValidTo { get; set; } = DateTime.Now.AddMinutes(5);

}
