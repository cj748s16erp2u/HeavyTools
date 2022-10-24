using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model.Base;

namespace eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model;


[Table("ols_tmppresorder")]
public class TmpPresorder : Entity
{

    [Key]
    [Column("resid")]
    public int Resid { get; set; }
     

    [Column("sordlineid", TypeName = "int")]
    public int? Sordlineid { get; set; }

}
