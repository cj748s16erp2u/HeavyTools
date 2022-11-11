using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Dto;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Model;
using eLog.HeavyTools.Services.WhZone.BusinessLogic.Containers.Interfaces;

namespace eLog.HeavyTools.Services.WhZone.BusinessLogic.Services.Interfaces;

public interface IWhZTranLocService : ILogicService<OlcWhztranloc>
{
    /// <summary>
    /// Zóna készlet tranzakció helykód lekérdezése
    /// </summary>
    /// <param name="predicate">Szűrési feltétel</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Tranzakció tételek listája</returns>
    Task<IEnumerable<WhZTranLocDto>> QueryAsync(WhZTranLocQueryDto query = null!, CancellationToken cancellationToken = default);

    /// <summary>
    /// Zóna készlet tranzakció helykód lekérés
    /// </summary>
    /// <param name="predicate">Szűrési feltétel</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Tranzakció tételek listája</returns>
    Task<WhZTranLocDto> GetAsync(int whztlocid, CancellationToken cancellationToken = default);

    /// <summary>
    /// Bevételezés alapértelmezett helykód bejegyzés létrehozása
    /// </summary>
    /// <param name="whZTranHead">Bevételezés tranzakció adatok</param>
    /// <param name="whZTranLine">Tétel adatok</param>
    /// <param name="context">Készletmozgás csomag adatok</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Létrehozott helykód bejegyzés</returns>
    Task<OlcWhztranloc?> AddReceivingDefaultIfNotExistsAsync(OlcWhztranhead whZTranHead, OlcWhztranline whZTranLine, IWhZStockMapContext context, CancellationToken cancellationToken = default);

    /// <summary>
    /// Bevételezés helykódok véglegesítése
    /// </summary>
    /// <param name="whZTranHead">Bevételezés tranzakció adatok</param>
    /// <param name="whZTranLine">Tétel adatok</param>
    /// <param name="context">Készletmozgás csomag adatok</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Véglegesített helykód bejegyzések</returns>
    Task<IEnumerable<OlcWhztranloc>> CommitReceivingAsync(OlcWhztranhead whZTranHead, OlcWhztranline whZTranLine, IWhZStockMapContext context, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lekérdezés, hogy az adott tétel azonosítóhoz tartozik-e helykód információ
    /// </summary>
    /// <param name="whztranlineid">Tétel azonosító</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Igen / Name</returns>
    Task<bool> AnyAsync(int whztranlineid, CancellationToken cancellationToken = default);

    /// <summary>
    /// Tételhez tartozó helykód információk törlése
    /// </summary>
    /// <param name="whztlineid">Tétel azonosító</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IEnumerable<WhZTranLocDto>> DeleteAllAsync(int whztlineid, CancellationToken cancellationToken = default);
}
