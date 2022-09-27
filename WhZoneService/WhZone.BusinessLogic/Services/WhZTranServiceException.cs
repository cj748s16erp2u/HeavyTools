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
public class WhZTranServiceException : LogicServiceException<OlcWhztranhead, WhZTranService>
{
    protected WhZTranServiceException() { }

    public WhZTranServiceException(WhZTranExceptionType type, OlcWhztranhead? entity, string? fieldName = null) : base(entity, fieldName) => this.Type = type;

    public WhZTranServiceException(WhZTranExceptionType type, string? message, OlcWhztranhead? entity, string? fieldName = null) : base(message, entity, fieldName) => this.Type = type;

    public WhZTranServiceException(WhZTranExceptionType type, string? message, Exception? innerException, OlcWhztranhead? entity, string? fieldName = null) : base(message, innerException, entity, fieldName) => this.Type = type;

    public WhZTranExceptionType Type { get; init; }
}
