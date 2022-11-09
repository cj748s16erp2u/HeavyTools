using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model.Base;
namespace eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model
{
    [Keyless]
    [Table("olc_sp_ols_getnewsordnum")]
    public partial class OlcSpOlsGetnewsordnum : Entity
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("result")]
        public int? Result { get; set; }
        [Column("docnum")]
        [StringLength(12)]
        [Unicode(false)]
        public string? Docnum { get; set; }
    }
}