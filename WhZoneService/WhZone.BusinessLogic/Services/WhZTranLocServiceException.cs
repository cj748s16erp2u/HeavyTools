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
public class WhZTranLocServiceException : LogicServiceException<OlcWhztranloc, WhZTranLocService>
{
    protected WhZTranLocServiceException()
    {
    }

    public WhZTranLocServiceException(WhZTranLocExceptionType type, OlcWhztranloc? entity, string? fieldName = null) : base(entity, fieldName) => this.Type = type;

    public WhZTranLocServiceException(WhZTranLocExceptionType type, string? message, OlcWhztranloc? entity, string? fieldName = null) : base(message, entity, fieldName) => this.Type = type;

    public WhZTranLocServiceException(WhZTranLocExceptionType type, string? message, Exception? innerException, OlcWhztranloc? entity, string? fieldName = null) : base(message, innerException, entity, fieldName) => this.Type = type;

    public WhZTranLocExceptionType Type { get; init; }
}
