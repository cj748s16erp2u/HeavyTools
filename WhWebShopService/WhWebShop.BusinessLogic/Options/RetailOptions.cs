using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Options;

public class RetailOptions
{
    public const string NAME = "Retail";
    public string? OrderSordDocId { get; set; }
    public string? Curid { get; set; }
    public string? Paymid { get; set; }
    public int GenId { get; internal set; }
}
