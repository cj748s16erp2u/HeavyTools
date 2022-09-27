using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model.Base;
using Microsoft.EntityFrameworkCore;

namespace eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model;

[Table("olc_pricecalcresult")]
public class OlcPriceCalcResult : BusinessEntity
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    [Column("value", TypeName = "numeric(19, 6)")]
    public decimal Value { get; set; }
}
