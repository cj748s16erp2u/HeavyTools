using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model.Interfaces;

public interface IBusinessMasterEntity : IBusinessEntity
{
    int Delstat { get; set; }
}
