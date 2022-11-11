using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Dto;

public class PartnerResultDto {
    public int? Partnid { get; set; } = null!;

    public int? SinvAddrid { get; set; } = null!;
    public int? ShippingAddrid { get; set; } = null!;



    public PartnerResultDto()
    {
    }

} 
