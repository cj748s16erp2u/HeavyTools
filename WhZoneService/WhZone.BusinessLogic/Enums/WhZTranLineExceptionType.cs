using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Dto;

namespace eLog.HeavyTools.Services.WhZone.BusinessLogic.Enums;

public enum WhZTranLineExceptionType
{
    /// <summary>
    /// A megadott <see cref="WhZTranLineDto.Whztlineid"/> és <see cref="WhZReceivingTranLineDto.Stlineid"/> alapján tranzakció tétel nem található
    /// </summary>
    EntryNotFound = 10200,
    /// <summary>
    /// A megadott <see cref="WhZReceivingTranLineDto.Stlineid"/> értéke null
    /// </summary>
    InvalidStlineid,
}
