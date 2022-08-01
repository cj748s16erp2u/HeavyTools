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
#pragma warning disable S3925 // "ISerializable" should be implemented correctly
public class WhZStockServiceException : LogicServiceException<OlcWhzstock, WhZStockService>
#pragma warning restore S3925 // "ISerializable" should be implemented correctly
{
    protected WhZStockServiceException()
    {
    }

    public WhZStockServiceException(WhZStockExceptionType type, OlcWhzstock? entity, string? fieldName = null) : base(entity, fieldName) => this.Type = type;

    public WhZStockServiceException(WhZStockExceptionType type, string? message, OlcWhzstock? entity, string? fieldName = null) : base(message, entity, fieldName) => this.Type = type;

    public WhZStockServiceException(WhZStockExceptionType type, string? message, Exception? innerException, OlcWhzstock? entity, string? fieldName = null) : base(message, innerException, entity, fieldName) => this.Type = type;

    public WhZStockExceptionType Type { get; init; }
}
