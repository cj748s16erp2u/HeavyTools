using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model.Base;
namespace eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model
{
    [Table("ols_recid")]
    public partial class OlsRecid : Entity
    {
        [Key]
        [Column("riid")]
        [StringLength(30)]
        [Unicode(false)]
        public string Riid { get; set; } = null!;
        [Column("lastid")]
        public int Lastid { get; set; }
    }
}