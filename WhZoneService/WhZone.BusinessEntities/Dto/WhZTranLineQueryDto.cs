using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Helpers;

namespace eLog.HeavyTools.Services.WhZone.BusinessEntities.Dto;

public class WhZTranLineQueryDto
{
    [QueryOperation(ExpressionType = System.Linq.Expressions.ExpressionType.Equal)]
    public int? Whztid { get; set; }

    [QueryOperation(ExpressionType = System.Linq.Expressions.ExpressionType.Equal)]
    public int? Whztlineid { get; set; }

    [QueryOperation(ExpressionType = System.Linq.Expressions.ExpressionType.Equal)]
    public int? Stid { get; set; }

    [QueryOperation(ExpressionType = System.Linq.Expressions.ExpressionType.Equal)]
    public int? Stlineid { get; set; }

    [QueryOperation(ExpressionType = System.Linq.Expressions.ExpressionType.Equal)]
    public int? Itemid { get; set; }
}
