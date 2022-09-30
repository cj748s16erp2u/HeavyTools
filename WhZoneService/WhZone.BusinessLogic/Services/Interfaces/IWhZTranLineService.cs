using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Dto;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Model;

namespace eLog.HeavyTools.Services.WhZone.BusinessLogic.Services.Interfaces;

public interface IWhZTranLineService : ILogicService<OlcWhztranline>
{
    /// <summary>
    /// Bevételezés típusú tranzakció tétel rögzítése
    /// </summary>
    /// <param name="request">Tranzakció tétel adatok</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Rögzített tranzakció tétel</returns>
    Task<WhZReceivingTranLineDto> AddReceivingAsync(WhZReceivingTranLineDto request, CancellationToken cancellationToken = default);
}
