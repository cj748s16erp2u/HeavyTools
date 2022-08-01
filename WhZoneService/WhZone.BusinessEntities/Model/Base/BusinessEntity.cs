using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Model.Interfaces;

namespace eLog.HeavyTools.Services.WhZone.BusinessEntities.Model.Base;

public class BusinessEntity : Entity, IBusinessEntity
{
    public string Addusrid { get; set; } = null!;
    public DateTime Adddate { get; set; }

    public virtual CfwUser Addusr { get; set; } = null!;
}
