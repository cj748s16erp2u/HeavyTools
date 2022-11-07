using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Dto;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Model;

namespace eLog.HeavyTools.Services.WhZone.BusinessLogic.Enums;

public enum WhZTranExceptionType
{
    /// <summary>
    /// A megadott <see cref="WhZTranHeadDto.Whztid"/> és <see cref="WhZReceivingTranHeadDto.Stid"/> alapján tranzakció nem található
    /// </summary>
    EntryNotFound = 10100,
    /// <summary>
    /// A megadott <see cref="WhZTranHeadDto.Cmpid"/> értéke hibás
    /// </summary>
    InvalidCmpid,
    /// <summary>
    /// A megadott <see cref="WhZTranHeadDto.Whztid"/> értéke null
    /// </summary>
    InvalidWhztid,
    /// <summary>
    /// A megadott <see cref="WhZTranHeadDto.Whzttype"/> értéke hibás
    /// </summary>
    InvalidWhzttype,
    /// <summary>
    /// A megadott <see cref="WhZReceivingTranHeadDto.Stid"/> értéke null
    /// </summary>
    InvalidStid,
    /// <summary>
    /// A megadott <see cref="WhZReceivingTranHeadDto.Towhzid"/> értéke null
    /// </summary>
    InvalidTowhzid,
    /// <summary>
    /// A megadott <see cref="WhZTranHeadDto.AuthUser"/> értéke null
    /// </summary>
    InvalidAuthUser,
    /// <summary>
    /// A megadott <see cref="WhZTranHeadStatChangeDto.Whztid"/> és <see cref="WhZTranHeadStatChangeDto.Stid"/> értéke null
    /// </summary>
    InvalidIdentifier,
}
