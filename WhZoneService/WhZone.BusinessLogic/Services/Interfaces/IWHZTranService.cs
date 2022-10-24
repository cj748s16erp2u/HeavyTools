using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Dto;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Model;

namespace eLog.HeavyTools.Services.WhZone.BusinessLogic.Services.Interfaces;

public interface IWhZTranService : ILogicService<OlcWhztranhead>
{
    /// <summary>
    /// Bevételezés típusú tranzakciók lekérdezése
    /// </summary>
    /// <param name="predicate">Szűrési feltétel</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Tranzakciók listája</returns>
    Task<IEnumerable<WhZReceivingTranHeadDto>> QueryReceivingAsync(WhZTranHeadQueryDto query = null!, CancellationToken cancellationToken = default);

    /// <summary>
    /// Bevételezés típusú tranzakció rögzítése
    /// </summary>
    /// <param name="request">Tranzakciós adatok</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Rögzített tranzakció</returns>
    Task<WhZReceivingTranHeadDto> AddReceivingAsync(WhZReceivingTranHeadDto request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Bevételezés típusú tranzakció módosítása
    /// </summary>
    /// <param name="request">Tranzakciós adatok</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Módosított tranzakció</returns>
    Task<WhZReceivingTranHeadDto> UpdateReceivingAsync(WhZReceivingTranHeadDto request, CancellationToken cancellationToken = default);

    Task<WhZTranHeadStatChangeResultDto> StatChangeAsync(WhZTranHeadStatChangeDto request, CancellationToken cancellationToken = default);

    Task<WhZTranHeadCloseResultDto> CloseAsync(WhZTranHeadCloseDto request, CancellationToken cancellationToken = default);
}