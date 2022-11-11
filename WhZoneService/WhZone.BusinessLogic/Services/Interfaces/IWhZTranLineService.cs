using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Dto;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Model;
using eLog.HeavyTools.Services.WhZone.BusinessLogic.Containers.Interfaces;

namespace eLog.HeavyTools.Services.WhZone.BusinessLogic.Services.Interfaces;

public interface IWhZTranLineService : ILogicService<OlcWhztranline>
{
    /// <summary>
    /// Bevételezés típusú tranzakció tételek lekérdezése
    /// </summary>
    /// <param name="predicate">Szűrési feltétel</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Tranzakció tételek listája</returns>
    Task<IEnumerable<WhZReceivingTranLineDto>> QueryReceivingAsync(WhZTranLineQueryDto query, CancellationToken cancellationToken = default);

    /// <summary>
    /// Bevételezés típusú tranzakció tétel rögzítése
    /// </summary>
    /// <param name="request">Tranzakció tétel adatok</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Rögzített tranzakció tétel</returns>
    Task<WhZReceivingTranLineDto> AddReceivingAsync(WhZReceivingTranLineDto request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Bevételezés típusú tranzakció tétel módosítása
    /// </summary>
    /// <param name="request">Tranzakció tétel adatok</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Módosított tranzakció tétel</returns>
    Task<WhZReceivingTranLineDto> UpdateReceivingAsync(WhZReceivingTranLineDto request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Bevételezés alapértelmezett helykód bejegyzések létrehozása
    /// </summary>
    /// <param name="whZTranHead">Bevételezés tranzakció adatok</param>
    /// <param name="context">Készletmozgás csomag adatok</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Létrehozott helykód bejegyzések listája</returns>
    Task<IEnumerable<OlcWhztranloc>> GenerateReceivingLocAsync(OlcWhztranhead whZTranHead, IWhZStockMapContext context, CancellationToken cancellationToken = default);

    /// <summary>
    /// Bevételezés helykódok véglegesítése
    /// </summary>
    /// <param name="whZTranHead">Bevételezés tranzakció adatok</param>
    /// <param name="context">Készletmozgás csomag adatok</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Véglegesített helykód bejegyzések</returns>
    Task<IEnumerable<OlcWhztranloc>> CommitReceivingLocAsync(OlcWhztranhead whZTranHead, IWhZStockMapContext context, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lekérdezés, hogy az adott fej azonosítóhoz tartozik-e tétel
    /// </summary>
    /// <param name="whztid">Fej azonosító</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Igen / Name</returns>
    Task<bool> AnyAsync(int whztid, CancellationToken cancellationToken = default);

    /// <summary>
    /// Bevételezés típusú tranzakció tétel törlése
    /// </summary>
    /// <param name="request">Tranzakció tétel adatok</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Törölt tranzakció tétel</returns>
    Task<WhZReceivingTranLineDto> DeleteAsync(WhZTranLineDeleteDto request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Tranzakcióhoz tartozó tétel törlése
    /// </summary>
    /// <param name="whztid">Tranzakció azonosító</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IEnumerable<WhZTranLineDto>> DeleteAllAsync(int whztid, CancellationToken cancellationToken = default);
}
