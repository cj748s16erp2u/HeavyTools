using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Services.WhZone.BusinessEntities.Dto;

public class WhZTranLocDto : Base.EntityDto
{
    public int? Whztlocid { get; set; }
    public int? Whztid { get; set; }
    public int? Whztlineid { get; set; }
    public string? Whid { get; set; }
    public string? Whname { get; set; }
    public int? Whzoneid { get; set; }
    public string? Whzonecode { get; set; }
    public string? Whzonename { get; set; }
    public int? Whlocid { get; set; }
    public string? Whloccode { get; set; }
    public string? Whlocname { get; set; }
    public int? Itemid { get; set; }
    public string? Itemcode { get; set; }
    public string? Itemname01 { get; set; }
    public int? Whztltype { get; set; }
    public decimal? Ordqty { get; set; }
    public decimal? Dispqty { get; set; }
    public decimal? Movqty { get; set; }
    public string? Addusrid { get; set; }
    public DateTime? Adddate { get; set; }
}
