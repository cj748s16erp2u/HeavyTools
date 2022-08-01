using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model.Base;

public class BusinessEntity : Entity, IBusinessEntity
{
    [Column("addusrid")]
    [StringLength(12)]
    [Unicode(false)]
    public string Addusrid { get; set; } = null!;
    [Column("adddate", TypeName = "datetime")]
    public DateTime Adddate { get; set; }

    [ForeignKey("Addusrid")]
    public virtual CfwUser Addusr { get; set; } = null!;
}
