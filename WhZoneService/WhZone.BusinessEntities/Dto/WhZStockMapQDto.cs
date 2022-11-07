using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace eLog.HeavyTools.Services.WhZone.BusinessEntities.Dto;

public class WhZStockMapQDto
{
    public int Itemid { get; init; }
    public string Whid { get; init; } = null!;
    public int? Whzoneid { get; init; }
    public int? Whlocid { get; init; }

    public decimal Recqty { get; set; }
    public decimal Reqqty { get; set; }
    public decimal Actqty { get; set; }
    public decimal Resqty { get; set; }
    public decimal Provqty { get; set; }
    public decimal Freeqty { get; set; }

    public string Whname { get; set; } = null!;
    public string Itemcode { get; set; } = null!;
    public string Itemname { get; set; } = null!;
    public string? Whzonecode { get; set; }
    public string? Whzonename { get; set; }
    public string Whloccode { get; set; } = null!;
    public string Whlocname { get; set; } = null!;
}
