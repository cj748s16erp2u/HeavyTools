using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Helpers;

namespace eLog.HeavyTools.Services.WhZone.BusinessEntities.Dto;

public class WhZTranHeadQueryDto
{
    [QueryOperation(ExpressionType = System.Linq.Expressions.ExpressionType.Equal)]
    public int? Whztid { get; set; }

    [QueryOperation(ExpressionType = System.Linq.Expressions.ExpressionType.Equal)]
    public int? Cmpid { get; set; }

    [QueryOperation(ExpressionType = System.Linq.Expressions.ExpressionType.GreaterThanOrEqual, FieldName = nameof(Model.OlcWhztranhead.Whztdate))]
    public DateTime? Fromdate { get; set; }

    [QueryOperation(ExpressionType = System.Linq.Expressions.ExpressionType.LessThanOrEqual, FieldName = nameof(Model.OlcWhztranhead.Whztdate))]
    public DateTime? Todate { get; set; }

    [QueryOperation(ExpressionType = System.Linq.Expressions.ExpressionType.Equal)]
    public int? Fromwhzid { get; set; }

    [QueryOperation(ExpressionType = System.Linq.Expressions.ExpressionType.Equal)]
    public int? Towhzid { get; set; }

    [QueryOperation(ExpressionType = System.Linq.Expressions.ExpressionType.Equal)]
    public int? Stid { get; set; }
}
