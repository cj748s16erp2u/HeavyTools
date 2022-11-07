using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Helpers;

namespace eLog.HeavyTools.Services.WhZone.BusinessEntities.Dto;

public class WhZTranLocQueryDto
{
    [QueryOperation(OperationType = QueryOperationType.Equal)]
    public int? Whztlocid { get; set; }
    
    [QueryOperation(OperationType = QueryOperationType.Equal)]
    public int? Whztid { get; set; }
    
    [QueryOperation(OperationType = QueryOperationType.Equal, FieldName = "Whzt.Stid")]
    public int? Stid { get; set; }
    
    [QueryOperation(OperationType = QueryOperationType.Equal)]
    public int? Whztlineid { get; set; }
    
    [QueryOperation(OperationType = QueryOperationType.Equal, FieldName = "Whztline.Stlineid")]
    public int? Stlineid { get; set; }
    
    [QueryOperation(OperationType = QueryOperationType.Like)]
    public string? Whid { get; set; }
    
    [QueryOperation(OperationType = QueryOperationType.Like, FieldName = "Wh.Name")]
    public string? Whname { get; set; }
    
    [QueryOperation(OperationType = QueryOperationType.Equal)]
    public int? Whzoneid { get; set; }
    
    [QueryOperation(OperationType = QueryOperationType.Like, FieldName = "Whzone.Whzonecode")]
    public string? Whzonecode { get; set; }
    
    [QueryOperation(OperationType = QueryOperationType.Like, FieldName = "Whzone.Name")]
    public string? Whzonename { get; set; }
    
    [QueryOperation(OperationType = QueryOperationType.Equal)]
    public int? Whlocid { get; set; }
    
    [QueryOperation(OperationType = QueryOperationType.Like, FieldName = "Whloc.Whloccode")]
    public string? Whloccode { get; set; }
    
    [QueryOperation(OperationType = QueryOperationType.Like, FieldName = "Whloc.Name")]
    public string? Whlocname { get; set; }
    
    [QueryOperation(OperationType = QueryOperationType.Like, FieldName = "Whztline.Item.Itemcode")]
    public string? Itemcode { get; set; }
    
    [QueryOperation(OperationType = QueryOperationType.Like, FieldName = "Whztline.Item.Name01")]
    public string? Itemname01 { get; set; }
}
