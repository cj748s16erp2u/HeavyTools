using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Services.WhZone.BusinessEntities.Dto;

public abstract class WhZTranLineDto : Base.EntityDto
{
    public int? Whztlineid { get; set; }
    public int? Whztid { get; set; }
    public int? Linenum { get; set; }
    public int? Itemid { get; set; }
    public decimal? Dispqty { get; init; }
    public decimal? Movqty { get; init; }
    public decimal? Inqty { get; init; }
    public decimal? Outqty { get; init; }
    public string Unitid2 { get; set; } = null!;
    public decimal? Change { get; set; }
    public decimal? Dispqty2 { get; set; }
    public decimal? Movqty2 { get; set; }
    public string? Note { get; set; }

    public string AuthUser { get; set; } = null!;
}
