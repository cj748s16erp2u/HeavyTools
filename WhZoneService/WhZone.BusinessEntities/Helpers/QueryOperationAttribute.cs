using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Services.WhZone.BusinessEntities.Helpers;

[System.Diagnostics.DebuggerStepThrough]
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public class QueryOperationAttribute : Attribute
{
    public ExpressionType ExpressionType { get; set; }
    public string? FieldName { get; set; }

    public QueryOperationAttribute() { }

    public QueryOperationAttribute(ExpressionType expressionType, string? fieldName)
    {
        this.ExpressionType = expressionType;
        this.FieldName = fieldName;
    }
}
