using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Helpers;

namespace eLog.HeavyTools.Services.WhZone.BusinessEntities.Dto;

public class WhZTranLineQueryDto
{
    [QueryOperation(OperationType = QueryOperationType.Equal)]
    public int? Whztid { get; set; }

    [QueryOperation(OperationType = QueryOperationType.Equal)]
    public int? Whztlineid { get; set; }

    [QueryOperation(OperationType = QueryOperationType.Equal)]
    public int? Stid { get; set; }

    [QueryOperation(OperationType = QueryOperationType.Equal)]
    public int? Stlineid { get; set; }

    [QueryOperation(OperationType = QueryOperationType.Equal)]
    public int? Itemid { get; set; }
}
