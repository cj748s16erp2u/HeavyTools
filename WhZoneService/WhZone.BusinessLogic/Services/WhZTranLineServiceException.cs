using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Model;
using eLog.HeavyTools.Services.WhZone.BusinessLogic.Enums;
using eLog.HeavyTools.Services.WhZone.BusinessLogic.Services.Base;

namespace eLog.HeavyTools.Services.WhZone.BusinessLogic.Services;

[System.Diagnostics.DebuggerStepThrough]
public class WhZTranLineServiceException : LogicServiceException<OlcWhztranline, WhZTranLineService>
{
    protected WhZTranLineServiceException() { }

    public WhZTranLineServiceException(WhZTranLineExceptionType type, OlcWhztranline? entity, string? fieldName = null) : base(entity, fieldName) => this.Type = type;

    public WhZTranLineServiceException(WhZTranLineExceptionType type, string? message, OlcWhztranline? entity, string? fieldName = null) : base(message, entity, fieldName) => this.Type = type;

    public WhZTranLineServiceException(WhZTranLineExceptionType type, string? message, Exception? innerException, OlcWhztranline? entity, string? fieldName = null) : base(message, innerException, entity, fieldName) => this.Type = type;

    public WhZTranLineExceptionType Type { get; init; }
}
