using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Dto;

namespace eLog.HeavyTools.Services.WhZone.BusinessLogic.Enums;

public enum WhZTranLocExceptionType
{
    /// <summary>
    /// A megadott <see cref="WhZTranLocDto.Whztlocid"/> alapján tranzakció helykód nem található.
    /// </summary>
    EntryNotFound = 10300,
    /// <summary>
    /// Az alapértelmezett betárolási helykód nincs paraméterezve.
    /// </summary>
    DefaultReceivingLocCodeNotSet,
    /// <summary>
    /// A megadott kód alapján helykód nem található.
    /// </summary>
    LocationNotFoundForCode,
    /// <summary>
    /// A megadott tételhez nem található helykód bejegyzés
    /// </summary>
    LocationsNotFoundForTranLine,
    /// <summary>
    /// A tétel rendelt mennyisége nem egyezik meg a helykódok össz mennyiségével
    /// </summary>
    LocationOrdQtyValueMismatch,
    /// <summary>
    /// A tétel kiírt mennyisége nem egyezik meg a helykódok össz mennyiségével
    /// </summary>
    LocationDispQtyValueMismatch,
    /// <summary>
    /// A tétel valós mennyisége nem egyezik meg a helykódok össz mennyiségével
    /// </summary>
    LocationMovQtyValueMismatch,
}
