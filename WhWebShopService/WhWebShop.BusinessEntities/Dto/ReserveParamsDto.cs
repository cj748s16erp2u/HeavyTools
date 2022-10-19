using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Dto.Attributes;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Dto;

[JsonObjectAttributes("Reserve")]
public class ReserveParamsDto
{

    [JsonField(MandotaryType.No)]
    public int? Resid { get; set; } = null!;
    [JsonField(MandotaryType.Yes)] 
    public int? Cmpid { get; set; } = null!;
    [JsonField(MandotaryType.Yes)] 
    public int? Partnid { get; set; } = null!;
    [JsonField(MandotaryType.Yes)] 
    public int? Addrid { get; set; } = null!;
    [JsonField(MandotaryType.Yes)] 
    public string Whid { get; set; } = null!;
    [JsonField(MandotaryType.Yes)] 
    public int? Itemid { get; set; } = null!;
    [JsonField(MandotaryType.No)] 
    public int? Lotid { get; set; } = null!;
    [JsonField(MandotaryType.Yes)] 
    public int? ResType { get; set; } = null!;
    [JsonField(MandotaryType.Yes)] 
    public decimal? ResQty { get; set; } = null!;
    [JsonField(MandotaryType.Yes)] 
    public DateTime? ResDate { get; set; } = null!;
    [JsonField(MandotaryType.No)] 
    public DateTime? FreeDate { get; set; } = null!;
    [JsonField(MandotaryType.No)] 
    public string Note { get; set; } = null!;
    [JsonField(MandotaryType.Yes)] 
    public string Addusrid { get; set; } = null!;
    [JsonField(MandotaryType.Yes)] 
    public DateTime? Adddate { get; set; } = null!;

    public OlsReserve ToOlsReserve(out bool isNew)
    {
        var n = new OlsReserve();
        if (this.Resid.HasValue)
        {
            isNew = false;
            n.Resid = this.Resid!.Value;
        }
        else
        {
            isNew = true;
        }

        n.Cmpid = this.Cmpid!.Value;
        n.Partnid = this.Partnid!.Value;
        n.Addrid = this.Addrid!.Value;
        n.Whid = this.Whid;
        n.Itemid = this.Itemid!.Value;
        n.Lotid = this.Lotid;
        n.Restype = this.ResType!.Value;
        n.Resqty = this.ResQty!.Value;
        n.Resdate = this.ResDate!.Value;
        n.Freedate = this.FreeDate;
        n.Note = this.Note;
        n.Addusrid = this.Addusrid;
        n.Adddate = this.Adddate!.Value;
        return n;
    }
}
