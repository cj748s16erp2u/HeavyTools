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
    public QueryOperationType OperationType { get; set; }
    public string? FieldName { get; set; }

    public QueryOperationAttribute() { }

    public QueryOperationAttribute(QueryOperationType operationType, string? fieldName)
    {
        this.OperationType = operationType;
        this.FieldName = fieldName;
    }
}

public enum QueryOperationType
{
    Custom,
    Equal,
    GreaterThan,
    GreaterThanOrEqual,
    LessThan,
    LessThanOrEqual,
    Like,
    MultipleAllowed
}
