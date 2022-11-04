using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Helpers;

namespace eLog.HeavyTools.Services.WhZone.BusinessEntities.Dto;

public class WhZTranHeadQueryDto
{
    [QueryOperation(OperationType = QueryOperationType.Equal)]
    public int? Whztid { get; set; }

    [QueryOperation(OperationType = QueryOperationType.Equal)]
    public int? Cmpid { get; set; }

    [QueryOperation(OperationType = QueryOperationType.GreaterThanOrEqual, FieldName = nameof(Model.OlcWhztranhead.Whztdate))]
    public DateTime? Fromdate { get; set; }

    [QueryOperation(OperationType = QueryOperationType.LessThanOrEqual, FieldName = nameof(Model.OlcWhztranhead.Whztdate))]
    public DateTime? Todate { get; set; }

    [QueryOperation(OperationType = QueryOperationType.Equal)]
    public int? Fromwhzid { get; set; }

    [QueryOperation(OperationType = QueryOperationType.Equal)]
    public int? Towhzid { get; set; }

    [QueryOperation(OperationType = QueryOperationType.Equal)]
    public int? Stid { get; set; }
}
