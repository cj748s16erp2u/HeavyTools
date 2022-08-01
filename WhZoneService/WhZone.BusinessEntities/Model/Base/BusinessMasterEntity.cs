using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Model.Interfaces;

namespace eLog.HeavyTools.Services.WhZone.BusinessEntities.Model.Base;

public class BusinessMasterEntity : BusinessEntity, IBusinessMasterEntity
{
    public int Delstat { get; set; }
}
