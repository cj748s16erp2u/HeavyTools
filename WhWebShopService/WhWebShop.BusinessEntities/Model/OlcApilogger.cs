using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model.Base;
namespace eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Model
{
    [Table("olc_apilogger")]
    public partial class OlcApilogger : Entity
    {
        [Key]
        [Column("apiid")]
        public int Apiid { get; set; }
        [Column("command")]
        [StringLength(100)]
        [Unicode(false)]
        public string Command { get; set; } = null!;
        [Column("request")]
        [Unicode(false)]
        public string Request { get; set; } = null!;
        [Column("response")]
        [Unicode(false)]
        public string? Response { get; set; }
    }
}