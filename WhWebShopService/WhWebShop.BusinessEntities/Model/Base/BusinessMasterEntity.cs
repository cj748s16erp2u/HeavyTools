using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model.Interfaces;

namespace eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model.Base;

public class BusinessMasterEntity : BusinessEntity, IBusinessMasterEntity
{
    [Column("delstat")]
    public int Delstat { get; set; }
}
