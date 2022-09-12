using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Dto;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Interfaces;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Model;
using eLog.HeavyTools.Services.WhZone.BusinessLogic.Containers.Interfaces;

namespace eLog.HeavyTools.Services.WhZone.BusinessLogic.Services.Interfaces;

public interface IWhZStockMapService : ILogicService<OlcWhzstockmap>
{
    /// <summary>
    /// Feldolgozási folyamat kontextus létrehozása
    /// </summary>
    /// <returns>A létrehozott kontextus</returns>
    IWhZStockMapContext CreateContext();

    /// <summary>
    /// 1 db készlet bejegyzés betöltése a megadott kulcs alapján.
    /// Több találat esetén kivétel kerül kiváltásra.
    /// </summary>
    /// <param name="key">Betöltendő kulcs</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>A betöltött bejegyzés vagy null</returns>
    /// <exception cref="InvalidOperationException"><paramref name="source" /> contains more than one element.</exception>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    ValueTask<OlcWhzstockmap?> GetAsync(IWhZStockMapKey key, CancellationToken cancellationToken = default);

    /// <summary>
    /// Új készlet beérkezés rögzítése
    /// </summary>
    /// <param name="context">Feldolgozási folyamat kontextus</param>
    /// <param name="request">Feldolgozandó kérés</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>A feldolgozott kérés adattartalma</returns>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    ValueTask<IWhZStockMapData> AddReceivingAsync(IWhZStockMapContext context, WhZStockMapDto request, CancellationToken cancellationToken = default);
    /// <summary>
    /// Meglévő készlet beérkezés törlése
    /// </summary>
    /// <param name="context">Feldolgozási folyamat kontextus</param>
    /// <param name="request">Feldolgozandó kérés</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>A feldolgozott kérés adattartalma</returns>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    ValueTask<IWhZStockMapData> RemoveReceivingAsync(IWhZStockMapContext context, WhZStockMapDto request, CancellationToken cancellationToken = default);
    /// <summary>
    /// Készlet beérkezés véglegesítése, tényleges készlet növelése
    /// </summary>
    /// <param name="context">Feldolgozási folyamat kontextus</param>
    /// <param name="request">Feldolgozandó kérés</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>A feldolgozott kérés adattartalma</returns>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    ValueTask<IWhZStockMapData> CommitReceivingAsync(IWhZStockMapContext context, WhZStockMapDto request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Új kiadás foglalás rögzítése
    /// </summary>
    /// <param name="context">Feldolgozási folyamat kontextus</param>
    /// <param name="request">Feldolgozandó kérés</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>A feldolgozott kérés adattartalma</returns>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    ValueTask<IWhZStockMapData> AddReservedAsync(IWhZStockMapContext context, WhZStockMapDto request, CancellationToken cancellationToken = default);
    /// <summary>
    /// Meglévő kiadás foglalás törlése
    /// </summary>
    /// <param name="context">Feldolgozási folyamat kontextus</param>
    /// <param name="request">Feldolgozandó kérés</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>A feldolgozott kérés adattartalma</returns>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    ValueTask<IWhZStockMapData> RemoveReservedAsync(IWhZStockMapContext context, WhZStockMapDto request, CancellationToken cancellationToken = default);
    /// <summary>
    /// Kiadás foglalás véglegesítése, tényleges készlet csökkentése
    /// </summary>
    /// <param name="context">Feldolgozási folyamat kontextus</param>
    /// <param name="request">Feldolgozandó kérés</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>A feldolgozott kérés adattartalma</returns>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    ValueTask<IWhZStockMapData> CommitReservedAsync(IWhZStockMapContext context, WhZStockMapDto request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Egy feldolgozott kérés kivétele a feldolgozási folyamatból
    /// </summary>
    /// <param name="context">Feldolgozási folyamat kontextus</param>
    /// <param name="request">Feldolgozott kérés</param>
    void Delete(IWhZStockMapContext context, IWhZStockMapData request);

    /// <summary>
    /// Feldolgozási folyamat véglegesítése
    /// Korábban kalkulált változások mentése az adatbázisba
    /// </summary>
    /// <param name="context">Feldolgozási folyamat kontextus</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    Task StoreAsync(IWhZStockMapContext context, CancellationToken cancellationToken = default);
}
