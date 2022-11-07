using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Model.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace eLog.HeavyTools.Services.WhZone.BusinessEntities.Model.Base;

public class BusinessEntity : Entity, IBusinessEntity
{
    [Column("addusrid")]
    [StringLength(12)]
    [Unicode(false)]
    public string Addusrid { get; set; } = null!;
    [Column("adddate", TypeName = "datetime")]
    public DateTime Adddate { get; set; }
}
