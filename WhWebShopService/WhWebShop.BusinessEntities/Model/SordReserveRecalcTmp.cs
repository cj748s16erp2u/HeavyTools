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
 


[Table("olc_sordreserverecalctmp")]
public class SordReserveRecalcTmp : BusinessEntity
{
    [Key]
    [Column("resid", TypeName = "int")]
    public int? Resid { get; set; }


    [Column("newresqty", TypeName = "numeric(19, 6)")]
    public decimal? Newresqty { get; set; }

}
