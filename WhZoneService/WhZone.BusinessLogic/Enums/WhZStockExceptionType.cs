using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Dto;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Model;
using eLog.HeavyTools.Services.WhZone.BusinessLogic.Containers.Interfaces;

namespace eLog.HeavyTools.Services.WhZone.BusinessLogic.Enums;

public enum WhZStockExceptionType
{
    /// <summary>
    /// A megadott <see cref="IWhZStockMapContext"/> példány null vagy nem megfelelő osztályból lett példányosítva
    /// </summary>
    InvalidContext = 10000,
    /// <summary>
    /// A megadott <see cref="WhZStockMapDto.Qty"/> értéke null vagy negatív érték
    /// </summary>
    InvalidRequestQty,
    /// <summary>
    /// Nincs <see cref="IWhZStockMapData"/> példány megadva
    /// </summary>
    InvalidRequestData,
    /// <summary>
    /// Nincs elegendő recQty, mint amennyivel csökkenteni kellene
    /// </summary>
    RemoveReceivingQty,
    /// <summary>
    /// Nincs elengedő recQty, mint amennyit véglegesíteni kellene
    /// </summary>
    CommitReceivingQty,
    /// <summary>
    /// Nincs elegendő actQty + recQty - resQty, mint amennyivel a foglalást növelni kellene
    /// </summary>
    AddReservedQty,
    /// <summary>
    /// Nincs elegendő resQty, mint amennyivel csokkenteni kellene
    /// </summary>
    RemoveReservedQty,
    /// <summary>
    /// Nincs elegendő resQty, mint amennyit véglegesíteni kellene
    /// </summary>
    CommitReservedQty,
    /// <summary>
    /// A bejegyzés nem távolítható el, mert nincs elég mennyiség
    /// </summary>
    DeleteNotEnoughQty,
    /// <summary>
    /// Az actQty, provQty nem lehet 0-tól kisebb érték
    /// </summary>
    CantHandleNegativStock,
    /// <summary>
    /// Új <see cref="OlcWhzstockmap"/> bejegyzés nem hozható létre, mert már létre lett hozva
    /// </summary>
    AlreadyExists,
    /// <summary>
    /// Meglévő <see cref="OlcWhzstockmap"/> bejegyzés nem módosítható, mert már módosítva lett
    /// </summary>
    AlreadyModified,
    /// <summary>
    /// Hiba történt az újonnan felvitt bejegyzés visszatöltése során
    /// </summary>
    InsertFailed,
}
