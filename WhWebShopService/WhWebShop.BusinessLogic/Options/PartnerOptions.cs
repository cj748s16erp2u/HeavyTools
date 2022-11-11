using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Services.WhWebShop.BusinessLogic.Options;

public class PartnerOptions
{
    public const string NAME = "Partner";
    public int? Cmpid { get; set; }
    public string? AddUsrId { get; set; }
    public int? PartnerType { get; set; }
    public string? Paymid { get; set; }
    public string? Curid { get; set; }
    public int? Credlimit { get; set; }
    public int? Selprcincdiscnttype { get; set; }
    public int? Posttype { get; set; } 
    public int? CmpType { get; set; }    
    public int? ShipAddressType { get; set; }
    public int? SinvAddressType { get; set; }
    public int? BackOrderType { get; set; }
}
