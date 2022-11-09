using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model;


[Table("olc_retailordertmptmp")]
public class RetailOrderTmp : BusinessEntity
{
    [Key]
    [Column("id", TypeName = "bigint")]
    public int? Id { get; set; } = null!;


    [Column("cmpid", TypeName = "int")]

    public int? Cmpid { get; set; } = null!;

    [Column("whid", TypeName = "varchar(12)")]
    public string? Whid { get; set; } = null!;

    [Column("addrid", TypeName = "int")]
    public int? Addrid { get; set; } = null!;

    [Column("partnid", TypeName = "int")]
    public int? Partnid { get; set; } = null!;

    [Column("itemid", TypeName = "int")]
    public int? Itemid { get; set; } = null!;
     
    [Column("ordqty", TypeName = "numeric(19, 6)")]
    public decimal? Ordqty { get; set; } = null!;

}
