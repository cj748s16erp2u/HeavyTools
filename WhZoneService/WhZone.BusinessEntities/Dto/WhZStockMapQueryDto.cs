using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Helpers;

namespace eLog.HeavyTools.Services.WhZone.BusinessEntities.Dto;

public class WhZStockMapQueryDto
{
    [QueryOperation(OperationType = QueryOperationType.Equal)]
    public int? Itemid { get; set; }
    
    [QueryOperation(OperationType = QueryOperationType.Like, FieldName = "Item.Itemcode")]
    public string? Itemcode { get; set; }
    
    [QueryOperation(OperationType = QueryOperationType.Like, FieldName = "Item.Name01")]
    public string? Itemname { get; set; }
    
    [QueryOperation(OperationType = QueryOperationType.Equal)]
    public string? Barcode { get; set; }
    
    [QueryOperation(OperationType = QueryOperationType.Custom)]
    public IEnumerable<int>? Cmpid { get; set; }
    
    [QueryOperation(OperationType = QueryOperationType.MultipleAllowed)]
    public IEnumerable<string>? Whid { get; set; }
    
    [QueryOperation(OperationType = QueryOperationType.Equal)]
    public int? Whzoneid { get; set; }
    
    [QueryOperation(OperationType = QueryOperationType.Like, FieldName = "Whzone.Whzonecode")]
    public string? Whzonecode { get; set; }
    
    [QueryOperation(OperationType = QueryOperationType.Equal)]
    public int? Whlocid { get; set; }
    
    [QueryOperation(OperationType = QueryOperationType.Like, FieldName = "Whloc.Whloccode")]
    public string? Whloccode { get; set; }
    
    [QueryOperation(OperationType = QueryOperationType.Equal)]
    public int? Delstat { get; set; }

    [QueryOperation(OperationType = QueryOperationType.Custom)]
    public bool? Nonzerostock { get; set; }
}
